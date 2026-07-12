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
public class LanguageLearnerController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICommandHandler<SetPracticeLanguageCommand> _setPracticeLanguageCommandHandler;
    private readonly IQueryHandler<GetUserLanguageQuery, string> _getUserLanguageQueryHandler;

    public LanguageLearnerController(ICurrentUserService currentUserService, 
        ICommandHandler<SetPracticeLanguageCommand> setPracticeLanguageCommandHandler,
        IQueryHandler<GetUserLanguageQuery, string> getUserLanguageQueryHandler)
    {
        _currentUserService = currentUserService;
        _setPracticeLanguageCommandHandler = setPracticeLanguageCommandHandler;
        _getUserLanguageQueryHandler = getUserLanguageQueryHandler;
    }

    [HttpGet(Name = "GetPracticeLanguage")]
    public async Task<ActionResult<string>> GetPracticeLanguage(CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }
       
        var query = new GetUserLanguageQuery(_currentUserService.UserId.Value);
        var practiceLanguage = await _getUserLanguageQueryHandler.HandleAsync(query, cancellationToken);

        return Ok(practiceLanguage);
    }


    [HttpPut(Name = "SetPracticeLanguage")]
    public async Task<ActionResult> SetPracticeLanguage(string languageCode)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var command = new SetPracticeLanguageCommand(_currentUserService.UserId.Value, languageCode);

        try
        {
            await _setPracticeLanguageCommandHandler.HandleAsync(command);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
