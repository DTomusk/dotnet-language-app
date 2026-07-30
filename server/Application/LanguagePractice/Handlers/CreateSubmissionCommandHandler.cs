using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Commands;
using Application.Submissions.Interfaces;
using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.Events;
using Domain.Shared.Results;

namespace Application.Submissions.Handlers;

public class CreateSubmissionCommandHandler : ICommandHandler<CreateSubmissionCommand, Guid>
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ILanguageLearnerRepository _languageLearnerRepository;
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILanguageValidationService _languageValidationService;

    public CreateSubmissionCommandHandler(
        ISubmissionRepository submissionRepository,
        ILanguageLearnerRepository languageLearnerRepository,
        IDomainEventPublisher eventPublisher,
        IUnitOfWork unitOfWork,
        ILanguageValidationService languageValidationService)
    {
        _submissionRepository = submissionRepository;
        _languageLearnerRepository = languageLearnerRepository;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
        _languageValidationService = languageValidationService;
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

        // Validate that the given text is likely to be in the expected language
        var validationResult = await _languageValidationService.ValidateTextInLanguageAsync(languageCode, command.Text, cancellationToken);
        if (validationResult.IsFailure)
            return Result<Guid>.Failure(new Error($"Text validation failed for language code: {languageCode}. Reason: {validationResult.Error.Message}", ErrorType.Validation));

        var submission = Submission.Create(command.UserID, languageCode, command.Text);

        await _submissionRepository.CreateAsync(submission, cancellationToken);

        var @event = new LanguageSubmissionCreatedEvent
        {
            SubmissionId = submission.Id,
            UserId = command.UserID,
            LanguageCode = languageCode.ToString(),
            SubmissionText = command.Text
        };

        await _eventPublisher.PublishAsync(@event, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(submission.Id);
    }
}
