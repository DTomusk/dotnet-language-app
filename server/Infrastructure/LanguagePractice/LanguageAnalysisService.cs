using Application.LanguagePractice.DTOs;
using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Infrastructure.LanguagePractice;

public class LanguageAnalysisService : ILanguageAnalysisService
{
    public Task<Result<AnalysisResponse>> AnalyseTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default)
    {
        // TODO: this is obviously very stupid
        var response = new AnalysisResponse(text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        return Task.FromResult(Result<AnalysisResponse>.Success(response));
    }
}
