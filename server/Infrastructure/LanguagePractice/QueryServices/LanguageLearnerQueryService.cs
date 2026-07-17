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
