using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Domain.LanguagePractice.Events;

namespace Application.LanguagePractice.Handlers;

public class SubmissionCreatedEventHandler : IEventHandler<LanguageSubmissionCreatedEvent>
{
    private readonly ILanguageAnalysisService _analysisService;

    public SubmissionCreatedEventHandler(ILanguageAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    public async Task HandleAsync(LanguageSubmissionCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        await _analysisService.AnalyseText(@event.LanguageCode, @event.SubmissionText);
    }
}
