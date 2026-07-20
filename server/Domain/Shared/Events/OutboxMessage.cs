namespace Domain.Shared.Events;

public class OutboxMessage
{
    private OutboxMessage() { } // Private constructor for EF Core

    public Guid Id {  get; private set; }
    public string EventType { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    public static OutboxMessage Create(DomainEvent domainEvent, string serializedPayload)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = domainEvent.EventType,
            Payload = serializedPayload,
            OccurredAt = DateTime.UtcNow,
            ProcessedAt = null,
            Error = null,
            RetryCount = 0
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
        RetryCount++;
    }
}
