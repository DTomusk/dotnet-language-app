using Api.Submissions.DTOs;
using Application.Auth.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.DTOs;
using Application.Submissions.Queries;
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
    private readonly IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> _getSubmissionsQueryHandler;

    public SubmissionController(
        IValidator<CreateSubmissionRequest> createSubmissionRequestValidator,
        ICommandHandler<CreateSubmissionCommand, Guid> createSubmissionCommandHandler,
        ICurrentUserService currentUserService,
        IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> getSubmissionsHandler)
    {
        _createSubmissionRequestValidator = createSubmissionRequestValidator;
        _createSubmissionCommandHandler = createSubmissionCommandHandler;
        _currentUserService = currentUserService;
        _getSubmissionsQueryHandler = getSubmissionsHandler;
    }

    [HttpGet(Name = "GetSubmissions")]
    public async Task<ActionResult<IEnumerable<SubmissionResponse>>> Get()
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var query = new GetSubmissionsQuery(_currentUserService.UserId.Value);

        var submissions = await _getSubmissionsQueryHandler.HandleAsync(query);

        return Ok(submissions);
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

        // TODO: handle errors
        var submissionID = await _createSubmissionCommandHandler.HandleAsync(command);

        return CreatedAtAction(nameof(Post), new { id = submissionID }, submissionID);
    }
}
