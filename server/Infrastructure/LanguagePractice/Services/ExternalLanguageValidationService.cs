using Application.LanguagePractice.DTOs;
using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Infrastructure.LanguagePractice.Services;

public class ExternalLanguageValidationService : IExternalLanguageValidationService, IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalLanguageValidationService> _logger;

    public ExternalLanguageValidationService(HttpClient httpClient, ILogger<ExternalLanguageValidationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<ValidationResponse>> ValidateTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending text for validation. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);

        try
        {
            var request = new
            {
                text,
                languageCode = languageCode.ToString()
            };

            var response = await _httpClient.PostAsJsonAsync(
               "/validate",
               request,
               cancellationToken);

            response.EnsureSuccessStatusCode();

            var validationResult = await response.Content.ReadFromJsonAsync<ValidationApiResponse>(cancellationToken: cancellationToken);

            if (validationResult is null)
            {
                _logger.LogError("Validation response was null for Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
                return Result<ValidationResponse>.Failure(new Error("Validation response was null.", ErrorType.Internal));
            }

            return Result<ValidationResponse>.Success(new ValidationResponse(validationResult.Valid));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed while validating text. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
            return Result<ValidationResponse>.Failure(Error.Internal($"HTTP request failed: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while validating text. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
            return Result<ValidationResponse>.Failure(Error.Internal($"Error occurred while validating text: {ex.Message}"));
        }
    }

    public async Task<HealthCheckResult> IsHealthy()
    {
        try
        {
            var response = await _httpClient.GetAsync("/health");
            return new HealthCheckResult("LanguageValidationService", response.IsSuccessStatusCode);
        }
        catch
        {
            return new HealthCheckResult("LanguageValidationService", false);
        }
    }

    private class ValidationApiResponse
    {
        public bool Valid { get; set; }
    }
}