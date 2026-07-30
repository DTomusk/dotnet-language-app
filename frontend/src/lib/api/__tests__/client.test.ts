import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { initApiClient, apiFetch } from '../client';
import { ApiError } from '../error';

// Mock the auth modules
vi.mock('../../auth/token', () => ({
    getToken: vi.fn(() => 'test-token'),
    clearToken: vi.fn(),
}));

vi.mock('../../auth/session', () => ({
    handleUnauthorized: vi.fn(),
}));

describe('API Client', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    describe('initApiClient', () => {
        it('should set the base URL when given a valid URL', () => {
            expect(() => initApiClient('https://api.example.com')).not.toThrow();
        });

        it('should throw an error when given an empty string', () => {
            expect(() => initApiClient('')).toThrow('API base URL must be provided');
        });

        it('should throw an error when given only whitespace', () => {
            expect(() => initApiClient('   ')).toThrow('API base URL must be provided');
        });

        it('should throw an error when given undefined', () => {
            expect(() => initApiClient(undefined as any)).toThrow('API base URL must be provided');
        });

        it('should trim the base URL', () => {
            expect(() => initApiClient('  https://api.example.com  ')).not.toThrow();
        });
    });

    describe('apiFetch', () => {
        beforeEach(() => {
            initApiClient('https://api.example.com');
            vi.stubGlobal('fetch', vi.fn());
        });

        afterEach(() => {
            vi.restoreAllMocks();
        });

        it('should successfully fetch and parse JSON response', async () => {
            const mockData = { id: 1, name: 'test' };
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(mockData)),
            });

            const result = await apiFetch('/test', {});
            expect(result).toEqual(mockData);
        });

        it('should handle empty response body', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce(''),
            });

            const result = await apiFetch('/test', {});
            expect(result).toBeNull();
        });

        it('should parse text response when JSON parsing fails', async () => {
            const plainText = 'Plain text response';
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce(plainText),
            });

            const result = await apiFetch('/test', {});
            expect(result).toBe(plainText);
        });

        it('should throw ApiError on failed response', async () => {
            const errorData = {
                title: 'One or more validation errors occurred.',
                status: 400,
                errors: {
                    Text: ["'Text' must not be empty."],
                },
            };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 400,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            await expect(apiFetch('/test', {})).rejects.toThrow(ApiError);
        });

        it('should include authorization header when token is available', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce('{}'),
            });

            await apiFetch('/test', {});

            const fetchCall = (fetch as any).mock.calls[0];
            const headers = fetchCall[1].headers;
            expect(headers.Authorization).toBe('Bearer test-token');
        });

        it('should serialize object body to JSON', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce('{}'),
            });

            const body = { text: 'hello' };
            await apiFetch('/test', { body: JSON.stringify(body) });

            const fetchCall = (fetch as any).mock.calls[0];
            expect(fetchCall[1].body).toBe(JSON.stringify(body));
            expect(fetchCall[1].headers['Content-Type']).toBe('application/json');
        });

        it('should not serialize FormData', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce('{}'),
            });

            const formData = new FormData();
            formData.append('file', new Blob(['content']), 'test.txt');

            await apiFetch('/test', { body: formData });

            const fetchCall = (fetch as any).mock.calls[0];
            expect(fetchCall[1].body).toBe(formData);
            expect(fetchCall[1].headers['Content-Type']).toBeUndefined();
        });

        it('should include credentials in fetch options', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce('{}'),
            });

            await apiFetch('/test', {});

            const fetchCall = (fetch as any).mock.calls[0];
            expect(fetchCall[1].credentials).toBe('include');
        });

        it('should use the initialized base URL', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: true,
                text: vi.fn().mockResolvedValueOnce('{}'),
            });

            await apiFetch('/test', {});

            const fetchCall = (fetch as any).mock.calls[0];
            const url = fetchCall[0];
            expect(url).toBe('https://api.example.com/test');
        });
    });

    describe('Error Handling', () => {
        beforeEach(() => {
            initApiClient('https://api.example.com');
            vi.stubGlobal('fetch', vi.fn());
        });

        it('should handle Fluent Validation errors with field errors', async () => {
            const errorData = {
                title: 'One or more validation errors occurred.',
                status: 400,
                errors: {
                    Text: ["'Text' must not be empty."],
                    Email: ["'Email' is invalid.", "'Email' is already in use."],
                },
            };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 400,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect(error).toBeInstanceOf(ApiError);
                expect((error as ApiError).message).toContain('Text: \'Text\' must not be empty.');
                expect((error as ApiError).message).toContain('Email: \'Email\' is invalid.');
                expect((error as ApiError).message).toContain('Email: \'Email\' is already in use.');
            }
        });

        it('should fallback to title when Fluent Validation has no field errors', async () => {
            const errorData = {
                title: 'Validation failed',
                status: 400,
                errors: {},
            };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 400,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).message).toBe('Validation failed');
            }
        });

        it('should handle error response with message property', async () => {
            const errorData = { message: 'Something went wrong' };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 500,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).message).toBe('Something went wrong');
            }
        });

        it('should handle error response with error property', async () => {
            const errorData = { error: 'Unauthorized' };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 401,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).message).toBe('Unauthorized');
            }
        });

        it('should handle plain text error responses', async () => {
            const errorText = 'Server error occurred';

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 500,
                text: vi.fn().mockResolvedValueOnce(errorText),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).message).toBe(errorText);
            }
        });

        it('should use default error message for empty error response', async () => {
            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 500,
                text: vi.fn().mockResolvedValueOnce(''),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).message).toBe('An error occurred');
            }
        });

        it('should include status code and data in ApiError', async () => {
            const errorData = { message: 'Test error' };

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 400,
                text: vi.fn().mockResolvedValueOnce(JSON.stringify(errorData)),
            });

            try {
                await apiFetch('/test', {});
                expect.fail('Should have thrown ApiError');
            } catch (error) {
                expect((error as ApiError).status).toBe(400);
                expect((error as ApiError).data).toEqual(errorData);
            }
        });

        it('should clear token on 401 response', async () => {
            const { clearToken } = await import('../../auth/token');

            (fetch as any).mockResolvedValueOnce({
                ok: false,
                status: 401,
                text: vi.fn().mockResolvedValueOnce('Unauthorized'),
            });

            try {
                await apiFetch('/test', {});
            } catch (error) {
                // Expected to throw
            }

            expect(clearToken).toHaveBeenCalled();
        });
    });
});
