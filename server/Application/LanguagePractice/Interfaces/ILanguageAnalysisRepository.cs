using Domain.LanguagePractice.Entities;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageAnalysisRepository
{
    Task<LanguageAnalysis> CreateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default);
    Task<LanguageAnalysis> UpdateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default);
}
