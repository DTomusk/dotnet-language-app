using Domain.LanguagePractice.ValueObjects;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageLearnerQueryService
{
    Task<LanguageCode?> GetUserLanguageAsync(Guid userId, CancellationToken cancellationToken = default);
}
