using Application.LanguagePractice.DTOs;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageAnalysisService
{
    Task<Result<AnalysisResponse>> AnalyseTextAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken = default);
}
