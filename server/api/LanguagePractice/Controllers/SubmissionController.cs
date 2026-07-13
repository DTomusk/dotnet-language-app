using Api.Shared.Controllers;
using Api.Shared.Extensions;
using Api.Submissions.DTOs;
using Application.Auth.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.DTOs;
using Application.Submissions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Submissions.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SubmissionController : AuthenticatedControllerBase
{
    private readonly ICommandHandler<CreateSubmissionCommand, Guid> _createSubmissionCommandHandler;
    private readonly IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> _getSubmissionsQueryHandler;

    public SubmissionController(
        ICommandHandler<CreateSubmissionCommand, Guid> createSubmissionCommandHandler,
        IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>> getSubmissionsHandler,
        ICurrentUserService currentUserService)
        : base(currentUserService)
    {
        _createSubmissionCommandHandler = createSubmissionCommandHandler;
        _getSubmissionsQueryHandler = getSubmissionsHandler;
    }

    [HttpGet(Name = "GetSubmissions")]
    public async Task<ActionResult<IEnumerable<SubmissionResponse>>> Get()
    {
        var query = new GetSubmissionsQuery(CurrentUserId);

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

        var command = new CreateSubmissionCommand(
            UserID: CurrentUserId,
            Text: req.Text);

        var submissionResult = await _createSubmissionCommandHandler.HandleAsync(command);

        return submissionResult.ToCreatedAtActionResult(
            actionName: nameof(Post),
            routeValues: new { id = submissionResult.Value });
    }
}
