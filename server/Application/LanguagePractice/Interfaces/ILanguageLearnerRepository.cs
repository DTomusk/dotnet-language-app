using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.ValueObjects;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageLearnerRepository
{
    Task<LanguageLearner?> GetByIdAsync(Guid languageLearnerId, CancellationToken cancellationToken = default);
    Task<LanguageLearner> CreateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default);
    Task<LanguageLearner> UpdateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default);
}
