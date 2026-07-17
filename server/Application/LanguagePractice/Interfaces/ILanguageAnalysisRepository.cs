using Domain.LanguagePractice.Entities;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageAnalysisRepository
{
    Task<LanguageAnalysis?> GetLanguageAnalysisAsync(Guid analysisId, CancellationToken cancellationToken = default);
    Task<LanguageAnalysis?> GetLanguageAnalysisBySubmissionIdAsync(Guid submissionId, CancellationToken cancellationToken = default);
    Task<LanguageAnalysis> CreateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default);
    Task<LanguageAnalysis> UpdateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default);
}
