using Application.LanguagePractice.DTOs;
using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LanguagePractice.QueryServices;

public class LanguageLearnerQueryService : ILanguageLearnerQueryService
{
    private readonly AppDbContext _context;

    public LanguageLearnerQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LanguageStatsDTO> GetLanguageStatsAsync(Guid userId, string languageCode, CancellationToken cancellationToken = default)
    {
        var result = await (from ll in _context.LanguageLearners.AsNoTracking()
                            join u in _context.Users on ll.UserId equals u.Id
                            where ll.UserId == userId
                            select new { ll, u })
            .ToListAsync(cancellationToken);

        var match = result
            .SelectMany(x => x.ll.LanguageStats, (x, stats) => new { x.u, stats })
            .FirstOrDefault(x => x.stats.LanguageCode.Value == languageCode);

        if (match == null)
            throw new InvalidOperationException($"Language stats not found for user {userId} and language {languageCode}");

        return new LanguageStatsDTO(
            match.u.DisplayName,
            match.stats.UniqueLemmas,
            match.stats.StartedLearningAt);
    }

    public async Task<LanguageCode?> GetUserLanguageAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageLearners
            .AsNoTracking()
            .Where(ll => ll.UserId == userId)
            .Select(ll => ll.ActiveLanguage)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<LemmaStatistic>> GetUserLemmaStatisticsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageLearners
            .AsNoTracking()
            .Where(ll => ll.UserId == userId)
            .SelectMany(ll => ll.LemmaStatistics)
            .ToListAsync(cancellationToken);
    }
}
