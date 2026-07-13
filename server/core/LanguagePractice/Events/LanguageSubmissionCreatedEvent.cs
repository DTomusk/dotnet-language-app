using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Events;

namespace Domain.LanguagePractice.Events;

public record LanguageSubmissionCreatedEvent : DomainEvent
{
    public required LanguageCode LanguageCode { get; init; }
    public required string SubmissionText { get; init; }
}
