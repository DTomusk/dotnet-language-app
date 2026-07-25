import { ApiError } from "./error";
import { getToken, clearToken } from "../auth/token"; 
import { handleUnauthorized } from "../auth/session";

let _baseUrl: string | undefined;

export function initApiClient(baseUrl: string) {
    const trimmed = baseUrl?.trim();
    if (!trimmed) throw new Error("API base URL must be provided");
    _baseUrl = trimmed;
}

// This function should be called for all API requests
export async function apiFetch<T>(
    endpoint: string, 
    options: RequestInit = {}
): Promise<T> {
    let token = getToken();
    let response = await sendRequest(endpoint, options, token);

    // TODO: once refreshes are implemented, check if initial response is 401
    // If so, try refreshing and calling again

    // Handle error response
    if (!response.ok) {
        await handleErrorResponse(response);
    }

    const data = await parseResponse(response);
    return data as T;
}

// calls fetch
async function sendRequest(
    endpoint: string,
    options: RequestInit,
    token: string | null,
) {
    const isFormData = options.body instanceof FormData;

    const headers = buildHeaders(options, isFormData, token);

    const shouldSerialize =
        options.body &&
        !isFormData &&
        typeof options.body === 'object' &&
        !(options.body instanceof Blob);

    const body = shouldSerialize
        ? JSON.stringify(options.body)
        : options.body;

    if (!_baseUrl) {
        throw new Error("API base URL is not initialized");
    }

    return fetch(`${_baseUrl}${endpoint}`, {
        ...options,
        headers,
        body,
        credentials: 'include',
    });
}

// Builds headers for request
function buildHeaders(
    options: RequestInit,
    isFormData: boolean,
    token: string | null,
) {
    const shouldSetJsonContentType =
        !isFormData && options.body;

    return {
        ...(options.headers || {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...(shouldSetJsonContentType ? { 'Content-Type': 'application/json' } : {}),
    };
}

async function parseResponse(response: Response) {
    const text = await response.text();

    if (!text) {
        return null;
    }

    try {
        return JSON.parse(text);
    } catch {
        return text;
    }
}

async function handleErrorResponse(response: Response) {
    if (response.status === 401) {
        clearToken();
        // Calls the function registered to handle unauthorized responses
        handleUnauthorized();
    }

    const errorText = await response.text();
    let errorData: any = {};

    try {
        errorData = errorText ? JSON.parse(errorText) : {};
    } catch (e) {
        // If response isn't JSON, use raw text
        errorData = { message: errorText || 'An error occurred' };
    }

    const errorMessage =
        errorData.error ||
        errorData.message ||
        'An error occurred';

    throw new ApiError(errorMessage, response.status, errorData);
}