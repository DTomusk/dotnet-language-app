using Api.Shared.Controllers;
using Api.Shared.Extensions;
using Application.Auth.Interfaces;
using Application.LanguagePractice.Commands;
using Application.LanguagePractice.Queries;
using Application.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.LanguagePractice.Controllers;

[ApiController]
[Authorize]
[Route("Me/Language")]
public class LanguageLearnerController : AuthenticatedControllerBase
{
    private readonly ICommandHandler<SetPracticeLanguageCommand> _setPracticeLanguageCommandHandler;
    private readonly IQueryHandler<GetUserLanguageQuery, string> _getUserLanguageQueryHandler;

    public LanguageLearnerController(ICurrentUserService currentUserService, 
        ICommandHandler<SetPracticeLanguageCommand> setPracticeLanguageCommandHandler,
        IQueryHandler<GetUserLanguageQuery, string> getUserLanguageQueryHandler)
        : base(currentUserService)
    {
        _setPracticeLanguageCommandHandler = setPracticeLanguageCommandHandler;
        _getUserLanguageQueryHandler = getUserLanguageQueryHandler;
    }

    [HttpGet(Name = "GetPracticeLanguage")]
    public async Task<IActionResult> GetPracticeLanguage(CancellationToken cancellationToken)
    {       
        var query = new GetUserLanguageQuery(CurrentUserId);
        var practiceLanguage = await _getUserLanguageQueryHandler.HandleAsync(query, cancellationToken);

        return Ok(practiceLanguage);
    }


    [HttpPut(Name = "SetPracticeLanguage")]
    public async Task<IActionResult> SetPracticeLanguage(string languageCode)
    {
        var command = new SetPracticeLanguageCommand(CurrentUserId, languageCode);

        var result = await _setPracticeLanguageCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}
