using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.Interfaces;
using Domain.LanguagePractice.Entities;
using Domain.Shared.Results;

namespace Application.Submissions.Handlers;

public class CreateSubmissionCommandHandler : ICommandHandler<CreateSubmissionCommand, Guid>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ILanguageLearnerRepository _languageLearnerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        ILanguageLearnerRepository languageLearnerRepository,
        IUnitOfWork unitOfWork)
    {
        _submissionRepository = submissionRepository;
        _languageLearnerRepository = languageLearnerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> HandleAsync(
        CreateSubmissionCommand command,
        CancellationToken cancellationToken = default)
    {
        var languageLearner = await _languageLearnerRepository.GetByIdAsync(command.UserID, cancellationToken);

        if (languageLearner == null)
            return Result<Guid>.Failure(new Error($"Language learner with ID {command.UserID} not found.", ErrorType.NotFound));

        var languageCode = languageLearner.ActiveLanguage;

        if (languageCode == null)
            return Result<Guid>.Failure(new Error($"Invalid language code: {languageCode}", ErrorType.Validation));

        var submission = Submission.Create(command.UserID, languageCode, command.Text);

        await _submissionRepository.CreateAsync(submission, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(submission.Id);
    }
}
