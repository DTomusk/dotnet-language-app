using Application.LanguagePractice.Interfaces;
using Application.LanguagePractice.Queries;
using Application.Shared.Interfaces;

namespace Application.LanguagePractice.Handlers;

public class GetUserLanguageQueryHandler : IQueryHandler<GetUserLanguageQuery, string>
{
    private readonly ILanguageLearnerQueryService _queryService;

    public GetUserLanguageQueryHandler(ILanguageLearnerQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<string> HandleAsync(GetUserLanguageQuery query, CancellationToken cancellationToken = default)
    {
        var languageCode = await _queryService.GetUserLanguageAsync(query.UserId, cancellationToken);
        return languageCode?.ToString() ?? string.Empty;
    }
}
