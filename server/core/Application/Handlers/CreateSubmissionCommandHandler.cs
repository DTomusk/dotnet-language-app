using Core.Application.Commands;
using Core.Application.Interfaces;
using Core.Domain;

namespace Core.Application.Handlers;

internal class CreateSubmissionCommandHandler : ICommandHandler<CreateSubmissionCommand, Guid>
{
    public async Task<Guid> HandleAsync(
        CreateSubmissionCommand command,
        CancellationToken cancellationToken = default)
    {
        var submission = new Submission(
            ID: Guid.NewGuid(),
            UserID: command.UserID,
            LanguageCode: command.LanguageCode,
            Text: command.Text);

        return submission.ID;
    }
}
