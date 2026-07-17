using Application.LanguagePractice.DTOs;
using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Infrastructure.LanguagePractice.Services;

public class LanguageAnalysisService : ILanguageAnalysisService, IHealthCheck
{
    private readonly HttpClient _httpClient;

    public LanguageAnalysisService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<Result<AnalysisResponse>> AnalyseTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default)
    {
        // TODO: this is obviously very stupid
        var response = new AnalysisResponse(text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return Task.FromResult(Result<AnalysisResponse>.Success(response));
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
}
