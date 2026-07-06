using Application.Commands;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Handlers;

internal class CreateSubmissionCommandHandler : ICommandHandler<CreateSubmissionCommand, Guid>
{
    public async Task<Guid> HandleAsync(
        CreateSubmissionCommand command,
        CancellationToken cancellationToken = default)
    {
        // Ensure language code is valid and supported
        // Persist submission in repo
        // Return ID of submission entity
        // Later, we'll want to evaluate the submission
        var submission = new Submission
        {
            ID = Guid.NewGuid(),
            UserID = command.UserID,
            LanguageCode = command.LanguageCode,
            Text = command.Text
        };

        return submission.ID;
    }
}
