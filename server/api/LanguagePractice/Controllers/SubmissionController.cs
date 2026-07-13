using Api.Shared.Extensions;
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
    private readonly ICommandHandler<CreateSubmissionCommand, Guid> _createSubmissionCommandHandler;
    private readonly ICurrentUserService _currentUserService;
    private readonly IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> _getSubmissionsQueryHandler;

    public SubmissionController(
        ICommandHandler<CreateSubmissionCommand, Guid> createSubmissionCommandHandler,
        ICurrentUserService currentUserService,
        IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> getSubmissionsHandler)
    {
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
    public async Task<IActionResult> Post(CreateSubmissionRequest req)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!_currentUserService.UserId.HasValue)
        {
            return Unauthorized(new { Message = "User not authenticated" });
        }

        var command = new CreateSubmissionCommand(
            UserID: _currentUserService.UserId.Value,
            Text: req.Text);

        var submissionResult = await _createSubmissionCommandHandler.HandleAsync(command);

        return submissionResult.ToCreatedAtActionResult(
            actionName: nameof(Post),
            routeValues: new { id = submissionResult.Value });
    }
}
