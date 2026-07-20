using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shared.Events;

public class ProcessedEvent
{
    private ProcessedEvent() { }

    public Guid EventId { get; private set; }
    public string HandlerName { get; private set; } = string.Empty;
    public DateTime ProcessedAt { get; private set; }

    public static ProcessedEvent Create(Guid eventId, string handlerName)
    {
        return new ProcessedEvent
        {
            EventId = eventId,
            HandlerName = handlerName,
            ProcessedAt = DateTime.UtcNow
        };
    }
}