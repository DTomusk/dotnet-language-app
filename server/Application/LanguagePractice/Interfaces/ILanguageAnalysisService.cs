using Application.LanguagePractice.DTOs;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageAnalysisService
{
    Task<Result<AnalysisResponse>> AnalyseText(LanguageCode languageCode, string text);
}
