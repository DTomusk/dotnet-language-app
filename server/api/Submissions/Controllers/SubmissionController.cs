using Api.Submissions.DTOs;
using Application.Auth.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.Queries;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Submissions.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SubmissionController : ControllerBase
{
    private readonly IValidator<CreateSubmissionRequest> _createSubmissionRequestValidator;
    private readonly ICommandHandler<CreateSubmissionCommand, Guid> _createSubmissionCommandHandler;
    private readonly ICurrentUserService _currentUserService;

    public SubmissionController(
        IValidator<CreateSubmissionRequest> createSubmissionRequestValidator,
        ICommandHandler<CreateSubmissionCommand, Guid> createSubmissionCommandHandler,
        ICurrentUserService currentUserService)
    {
        _createSubmissionRequestValidator = createSubmissionRequestValidator;
        _createSubmissionCommandHandler = createSubmissionCommandHandler;
        _currentUserService = currentUserService;
    }

    [HttpGet(Name = "GetSubmissions")]
    public ActionResult<IEnumerable<Submission>> Get()
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var query = new GetSubmissionsQuery(_currentUserService.UserId.Value);

        return Ok();
    }

    [HttpPost(Name = "CreateSubmission")]
    public async Task<ActionResult> Post(CreateSubmissionRequest req)
    {
        var validationResult = await _createSubmissionRequestValidator.ValidateAsync(req);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var command = new CreateSubmissionCommand(
            UserID: _currentUserService.UserId.Value,
            LanguageCode: req.LanguageCode,
            Text: req.Text);

        var submissionID = await _createSubmissionCommandHandler.HandleAsync(command);

        return CreatedAtAction(nameof(Post), new { id = submissionID }, submissionID);
    }
}
