using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.Interfaces;
using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.ValueObjects;

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

    public async Task<Guid> HandleAsync(
        CreateSubmissionCommand command,
        CancellationToken cancellationToken = default)
    {
        var languageLearner = await _languageLearnerRepository.GetByIdAsync(command.UserID, cancellationToken);

        if (languageLearner == null)
        {
            // TODO: throw a more relevant exception
            throw new ArgumentException($"Language learner with ID {command.UserID} not found.");
        }

        var languageCode = languageLearner.ActiveLanguage;

        if (languageCode == null)
        {
            throw new ArgumentException($"Invalid language code: {languageCode}");
        }

        var submission = Submission.Create(command.UserID, languageCode, command.Text);

        await _submissionRepository.CreateAsync(submission, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return submission.Id;
    }
}
