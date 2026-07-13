using Application.LanguagePractice.Commands;
using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Application.LanguagePractice.Handlers;

public class SetPracticeLanguageCommandHandler : ICommandHandler<SetPracticeLanguageCommand>
{
    private readonly ILanguageLearnerRepository _languageLearnerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetPracticeLanguageCommandHandler(ILanguageLearnerRepository languageLearnerRepository,
        IUnitOfWork unitOfWork)
    {
        _languageLearnerRepository = languageLearnerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(SetPracticeLanguageCommand command, CancellationToken cancellationToken)
    {
        var languageCode = LanguageCode.From(command.LanguageCode);
        var languageLearner = await _languageLearnerRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (languageLearner == null)
            return Result.Failure(new Error($"Language learner with user ID {command.UserId} not found.", ErrorType.NotFound));

        languageLearner.SetActiveLanguage(languageCode);
        await _languageLearnerRepository.UpdateAsync(languageLearner, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
