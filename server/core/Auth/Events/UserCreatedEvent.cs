using Domain.Shared.Events;

namespace Domain.Auth.Events;

public record UserCreatedEvent : DomainEvent
{
    public required Guid UserId { get; init; }
    public required string DisplayName { get; init; }
    public required DateTime CreatedAt { get; init; }
}
