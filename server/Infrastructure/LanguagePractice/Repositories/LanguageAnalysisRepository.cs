using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.Entities;
using Infrastructure.Shared;

namespace Infrastructure.LanguagePractice.Repositories;

public class LanguageAnalysisRepository : ILanguageAnalysisRepository
{
    private readonly AppDbContext _context;

    public LanguageAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LanguageAnalysis?> GetLanguageAnalysisAsync(Guid analysisId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageAnalysis.FindAsync(new object[] { analysisId }, cancellationToken);
    }

    public async Task<LanguageAnalysis> CreateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default)
    {
        _context.LanguageAnalysis.Add(languageAnalysis);
        return languageAnalysis;
    }

    public async Task<LanguageAnalysis?> GetLanguageAnalysisBySubmissionIdAsync(Guid submissionId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageAnalysis.FindAsync(new object[] { submissionId }, cancellationToken);
    }

    public async Task<LanguageAnalysis> UpdateLanguageAnalysisAsync(LanguageAnalysis languageAnalysis, CancellationToken cancellation = default)
    {
        _context.LanguageAnalysis.Update(languageAnalysis);
        return languageAnalysis;
    }
}
