using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Events;

namespace Domain.LanguagePractice.Events;

public record LanguageSubmissionCreatedEvent : DomainEvent
{
    public Guid SubmissionId { get; init; }
    public Guid UserId { get; init; }
    public required LanguageCode LanguageCode { get; init; }
    public required string SubmissionText { get; init; }
}
