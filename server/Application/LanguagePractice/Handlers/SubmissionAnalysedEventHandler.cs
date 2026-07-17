using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.Events;

namespace Application.LanguagePractice.Handlers;

public class SubmissionAnalysedEventHandler : IEventHandler<LanguageSubmissionAnalysedEvent>
{
    private readonly ILanguageAnalysisRepository _analysisRepo;
    private readonly ILanguageLearnerRepository _learnerRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SubmissionAnalysedEventHandler(
        ILanguageAnalysisRepository analysisRepo,
        ILanguageLearnerRepository learnerRepo,
        IUnitOfWork unitOfWork)
    {
        _analysisRepo = analysisRepo;
        _learnerRepo = learnerRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(LanguageSubmissionAnalysedEvent @event, CancellationToken cancellationToken = default)
    {
        var analysis = await _analysisRepo.GetLanguageAnalysisAsync(@event.AnalysisId, cancellationToken);
        if (analysis == null)
        {
            throw new InvalidOperationException($"Language analysis with ID {@event.AnalysisId} not found.");
        }

        if (analysis.Lemmas == null || !analysis.Lemmas.Any())
        {
            return; // No lemmas to process, exit early
        }

        var languageLearner = await _learnerRepo.GetByIdAsync(analysis.UserId, cancellationToken);

        if (languageLearner == null)
        {
            throw new InvalidOperationException($"Language learner with ID {analysis.UserId} not found.");
        }

        languageLearner.UpdateLemmaStatistics(analysis.Lemmas);
        await _learnerRepo.UpdateAsync(languageLearner, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
