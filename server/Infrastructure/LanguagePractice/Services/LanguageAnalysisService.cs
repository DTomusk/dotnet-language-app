using Application.LanguagePractice.DTOs;
using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Infrastructure.LanguagePractice.Services;

public class LanguageAnalysisService : ILanguageAnalysisService, IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LanguageAnalysisService> _logger;

    public LanguageAnalysisService(HttpClient httpClient, ILogger<LanguageAnalysisService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<AnalysisResponse>> AnalyseTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending text for analysis. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
        try
        {
            // Create request DTO
            var request = new
            {
                languageCode = languageCode.ToString(),
                text
            };

            // Send POST request with JSON body
            var response = await _httpClient.PostAsJsonAsync(
                "/analyze",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            // Deserialize response
            var apiResponse = await response.Content.ReadFromJsonAsync<AnalysisApiResponse>(cancellationToken: cancellationToken);

            if (apiResponse == null)
            {
                return Result<AnalysisResponse>.Failure(Error.Internal("Failed to deserialize API response"));
            }

            // Map to your AnalysisResponse DTO
            _logger.LogInformation("Received analysis response. Language: {LanguageCode}, Result Length: {ResultLength}", languageCode, apiResponse.Lemmas.Count);
            var analysisResponse = new AnalysisResponse(apiResponse.Lemmas);
            return Result<AnalysisResponse>.Success(analysisResponse);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed while analyzing text. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
            return Result<AnalysisResponse>.Failure(Error.Internal($"HTTP request failed: {ex.Message}"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while analyzing text. Language: {LanguageCode}, Text Length: {TextLength}", languageCode, text.Length);
            return Result<AnalysisResponse>.Failure(Error.Internal($"Error occurred while analyzing text: {ex.Message}"));
        }
    }

    public async Task<HealthCheckResult> IsHealthy()
    {
        try
        {
            var response = await _httpClient.GetAsync("/health");
            return new HealthCheckResult("LanguageAnalysisService", response.IsSuccessStatusCode);
        }
        catch
        {
            return new HealthCheckResult("LanguageAnalysisService", false);
        }
    }

    private class AnalysisApiResponse
    {
        public List<string> Lemmas { get; set; } = new List<string>();
    }
}
