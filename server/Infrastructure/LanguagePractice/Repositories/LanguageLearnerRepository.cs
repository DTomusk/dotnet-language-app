using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.Entities;
using Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.LanguagePractice.Repositories;

public class LanguageLearnerRepository : ILanguageLearnerRepository
{
    private readonly AppDbContext _context;

    public LanguageLearnerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LanguageLearner?> GetByIdAsync(Guid languageLearnerId, CancellationToken cancellationToken = default)
    {
        return await _context.LanguageLearners.FindAsync([languageLearnerId], cancellationToken);
    }

    public async Task<LanguageLearner> CreateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default)
    {
        _context.LanguageLearners.Add(languageLearner);
        return languageLearner;
    }

    public async Task<LanguageLearner> UpdateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default)
    {
        _context.LanguageLearners.Update(languageLearner);
        return languageLearner;
    }
}
