using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.Entities;
using Infrastructure.Shared;

namespace Infrastructure.LanguagePractice;

public class LanguageLearnerRepository : ILanguageLearnerRepository
{
    private readonly AppDbContext _context;

    public LanguageLearnerRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<LanguageLearner> CreateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default)
    {
        _context.LanguageLearners.Add(languageLearner);
        return languageLearner;
    }
}
