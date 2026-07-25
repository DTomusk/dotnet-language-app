using Api.Shared.RateLimiting;
using Api.Shared.Controllers;
using Api.Shared.Extensions;
using Application.Auth.Interfaces;
using Application.LanguagePractice.Commands;
using Application.LanguagePractice.Queries;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Api.LanguagePractice.DTOs;
using Application.LanguagePractice.DTOs;

namespace Api.LanguagePractice.Controllers;

[ApiController]
[Authorize]
[Route("Me/Language")]
[EnableRateLimiting(RateLimitingConfiguration.AuthenticatedPolicy)]
public class LanguageLearnerController : AuthenticatedControllerBase
{
    private readonly ICommandHandler<SetPracticeLanguageCommand> _setPracticeLanguageCommandHandler;
    private readonly IQueryHandler<GetUserLanguageQuery, string> _getUserLanguageQueryHandler;
    private readonly IQueryHandler<GetLemmaStatsQuery, IEnumerable<LemmaStatistic>> _getLemmaStatsQueryHandler;
    private readonly IQueryHandler<GetActiveLanguageStatsQuery, LanguageStatsResponse> _getActiveLanguageStatsQueryHandler;

    public LanguageLearnerController(ICurrentUserService currentUserService, 
        ICommandHandler<SetPracticeLanguageCommand> setPracticeLanguageCommandHandler,
        IQueryHandler<GetUserLanguageQuery, string> getUserLanguageQueryHandler,
        IQueryHandler<GetLemmaStatsQuery, IEnumerable<LemmaStatistic>> getLemmaStatsQueryHandler,
        IQueryHandler<GetActiveLanguageStatsQuery, LanguageStatsResponse> getActiveLanguageStatsQueryHandler)
        : base(currentUserService)
    {
        _setPracticeLanguageCommandHandler = setPracticeLanguageCommandHandler;
        _getUserLanguageQueryHandler = getUserLanguageQueryHandler;
        _getLemmaStatsQueryHandler = getLemmaStatsQueryHandler;
        _getActiveLanguageStatsQueryHandler = getActiveLanguageStatsQueryHandler;
    }

    [HttpGet(Name = "GetPracticeLanguage")]
    public async Task<IActionResult> GetPracticeLanguage(CancellationToken cancellationToken)
    {       
        var query = new GetUserLanguageQuery(CurrentUserId);
        var practiceLanguage = await _getUserLanguageQueryHandler.HandleAsync(query, cancellationToken);

        return Ok(practiceLanguage);
    }


    [HttpPut(Name = "SetPracticeLanguage")]
    public async Task<IActionResult> SetPracticeLanguage([FromBody] SetLanguageRequest request)
    {
        var command = new SetPracticeLanguageCommand(CurrentUserId, request.LanguageCode);

        var result = await _setPracticeLanguageCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }

    [HttpGet("LemmaStats", Name = "GetLemmaStats")]
    public async Task<IActionResult> GetLemmaStats(CancellationToken cancellationToken)
    {
        var query = new GetLemmaStatsQuery(CurrentUserId);
        var lemmaStats = await _getLemmaStatsQueryHandler.HandleAsync(query, cancellationToken);
        return Ok(lemmaStats);
    }

    [HttpGet("Stats", Name = "GetActiveLanguageStats")]
    public async Task<IActionResult> GetActiveLanguageStats(CancellationToken cancellationToken)
    {
        var query = new GetActiveLanguageStatsQuery(CurrentUserId);
        var stats = await _getActiveLanguageStatsQueryHandler.HandleAsync(query, cancellationToken);
        return Ok(stats);
    }
}
