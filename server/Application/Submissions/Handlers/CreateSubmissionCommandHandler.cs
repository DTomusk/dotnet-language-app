using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.Interfaces;
using Domain.Entities;

namespace Application.Submissions.Handlers;

public class CreateSubmissionCommandHandler : ICommandHandler<CreateSubmissionCommand, Guid>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        IUnitOfWork unitOfWork)
    {
        _submissionRepository = submissionRepository;
        _unitOfWork = unitOfWork;
    }

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

        await _submissionRepository.CreateAsync(submission, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return submission.ID;
    }
}
