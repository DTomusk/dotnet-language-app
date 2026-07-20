namespace Domain.Shared.Events;

/// <summary>
/// Base domain event class that provides common properties for all domain events
/// Can be extended with event specific properties
/// </summary>
public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    public string EventType { get; init; }

    protected DomainEvent()
    {
        EventType = GetType().Name;
    }
}
