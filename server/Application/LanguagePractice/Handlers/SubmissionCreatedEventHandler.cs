using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.Events;
using Domain.LanguagePractice.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Application.LanguagePractice.Handlers;

public class SubmissionCreatedEventHandler : IEventHandler<LanguageSubmissionCreatedEvent>
{
    private readonly ILanguageAnalysisService _analysisService;
    private readonly ILanguageAnalysisRepository _analysisRepo;
    private readonly ILogger<SubmissionCreatedEventHandler> _logger;

    public SubmissionCreatedEventHandler(ILanguageAnalysisService analysisService,
        ILanguageAnalysisRepository analysisRepo,
        ILogger<SubmissionCreatedEventHandler> logger)
    {
        _analysisService = analysisService;
        _analysisRepo = analysisRepo;
        _logger = logger;
    }

    public async Task HandleAsync(LanguageSubmissionCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling LanguageSubmissionCreatedEvent for SubmissionId: {SubmissionId}", @event.SubmissionId);

        var languageCode = LanguageCode.From(@event.LanguageCode);

        // Idempotency: check we haven't already analysed this submission
        var existingAnalysis = await _analysisRepo.GetLanguageAnalysisBySubmissionIdAsync(@event.SubmissionId, cancellationToken);
        if (existingAnalysis != null && existingAnalysis.Status == LanguageAnalysisStatus.Completed)
        {
            _logger.LogInformation("LanguageAnalysis already completed for SubmissionId: {SubmissionId}", @event.SubmissionId);
            return;
        }

        if (existingAnalysis == null)
        {
            var analysisCreateResult = LanguageAnalysis.Create(@event.SubmissionId, @event.UserId, languageCode);

            if (!analysisCreateResult.IsSuccess)
            {
                _logger.LogError("Failed to create LanguageAnalysis for SubmissionId: {SubmissionId}. Error: {Error}", @event.SubmissionId, analysisCreateResult.Error);
                throw new InvalidOperationException($"Failed to create LanguageAnalysis for SubmissionId: {@event.SubmissionId}. Error: {analysisCreateResult.Error}");
            }

            existingAnalysis = await _analysisRepo.CreateLanguageAnalysisAsync(analysisCreateResult.Value, cancellationToken);
        }

        var responseResult = await _analysisService.AnalyseTextAsync(languageCode, @event.SubmissionText, cancellationToken);

        if (responseResult.IsFailure)
        {
            existingAnalysis.MarkAsFailed();
            await _analysisRepo.UpdateLanguageAnalysisAsync(existingAnalysis, cancellationToken);
            throw new InvalidOperationException("Language analysis service returned a failure response.");
        }

        // Update the analysis entity with the results and persist 
        // Raise new event for processing user's vocabulary
    }
}
