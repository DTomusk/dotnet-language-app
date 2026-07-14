using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.Entities;

namespace Infrastructure.LanguagePractice.Repositories;

public class LanguageAnalysisRepository : ILanguageAnalysisRepository
{
    public Task<LanguageAnalysis> CreateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<LanguageAnalysis?> GetLanguageAnalysisBySubmissionIdAsync(Guid submissionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<LanguageAnalysis> UpdateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}
