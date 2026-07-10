using Domain.LanguagePractice.Entities;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageLearnerRepository
{
    Task<LanguageLearner> CreateAsync(LanguageLearner languageLearner, CancellationToken cancellationToken = default);
}
