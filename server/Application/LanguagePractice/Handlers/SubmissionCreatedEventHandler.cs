using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.Events;
using Microsoft.Extensions.Logging;

namespace Application.LanguagePractice.Handlers;

public class SubmissionCreatedEventHandler : IEventHandler<LanguageSubmissionCreatedEvent>
{
    private readonly ILanguageAnalysisService _analysisService;
    private readonly ILogger<SubmissionCreatedEventHandler> _logger;

    public SubmissionCreatedEventHandler(ILanguageAnalysisService analysisService,
        ILogger<SubmissionCreatedEventHandler> logger)
    {
        _analysisService = analysisService;
        _logger = logger;
    }

    public async Task HandleAsync(LanguageSubmissionCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling LanguageSubmissionCreatedEvent for SubmissionId: {SubmissionId}", @event.SubmissionId);

        var analysisCreateResult = LanguageAnalysis.Create(@event.SubmissionId, @event.UserId, @event.LanguageCode);

        if (!analysisCreateResult.IsSuccess)
        {
            _logger.LogError("Failed to create LanguageAnalysis for SubmissionId: {SubmissionId}. Error: {Error}", @event.SubmissionId, analysisCreateResult.Error);
            throw new InvalidOperationException($"Failed to create LanguageAnalysis for SubmissionId: {@event.SubmissionId}. Error: {analysisCreateResult.Error}");
        }

        var responseResult = await _analysisService.AnalyseText(@event.LanguageCode, @event.SubmissionText);
    }
}
