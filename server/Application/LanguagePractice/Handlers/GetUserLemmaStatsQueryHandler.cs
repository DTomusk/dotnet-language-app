using Application.LanguagePractice.Interfaces;
using Application.LanguagePractice.Queries;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;

namespace Application.LanguagePractice.Handlers;

public class GetUserLemmaStatsQueryHandler : IQueryHandler<GetLemmaStatsQuery, IEnumerable<LemmaStatistic>>
{
    private readonly ILanguageLearnerQueryService _queryService;

    public GetUserLemmaStatsQueryHandler(ILanguageLearnerQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<IEnumerable<LemmaStatistic>> HandleAsync(GetLemmaStatsQuery query, CancellationToken cancellationToken = default)
    {
        return await _queryService.GetUserLemmaStatisticsAsync(query.UserId, cancellationToken);
    }
}
