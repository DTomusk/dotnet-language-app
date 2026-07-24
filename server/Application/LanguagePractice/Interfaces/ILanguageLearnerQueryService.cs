using Application.LanguagePractice.DTOs;
using Domain.LanguagePractice.ValueObjects;

namespace Application.LanguagePractice.Interfaces;

public interface ILanguageLearnerQueryService
{
    Task<LanguageCode?> GetUserLanguageAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LemmaStatistic>> GetUserLemmaStatisticsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<LanguageStatsResponse> GetLanguageStatsAsync(Guid userId, string languageCode, CancellationToken cancellationToken = default);
}
