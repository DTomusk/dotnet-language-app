using Api.DTOs;
using Core.Application.Commands;
using Core.Application.Interfaces;
using Core.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubmissionController : ControllerBase
{
    private readonly IValidator<CreateSubmissionRequest> _createSubmissionRequestValidator;
    private readonly ICommandHandler<CreateSubmissionCommand, Guid> _createSubmissionCommandHandler;

    public SubmissionController(
        IValidator<CreateSubmissionRequest> createSubmissionRequestValidator,
        ICommandHandler<CreateSubmissionCommand, Guid> createSubmissionCommandHandler)
    {
        _createSubmissionRequestValidator = createSubmissionRequestValidator;
        _createSubmissionCommandHandler = createSubmissionCommandHandler;
    }

    [HttpGet(Name = "GetSubmissions")]
    public ActionResult<IEnumerable<Submission>> Get()
    {
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

        var command = new CreateSubmissionCommand(
            UserID: Guid.NewGuid(),
            LanguageCode: req.LanguageCode,
            Text: req.Text);

        var submissionID = await _createSubmissionCommandHandler.HandleAsync(command);

        return CreatedAtAction(nameof(Post), new { id = submissionID }, submissionID);
    }
}
