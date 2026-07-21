using Domain.Shared.Events;

namespace Domain.LanguagePractice.Events;

public record LanguageSubmissionAnalysedEvent : DomainEvent
{
    public Guid AnalysisId { get; init; }
}