using Application.Shared.Interfaces;
using Domain.Shared.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Shared.Events;

public class IdempotencyService : IIdempotencyService
{
    private readonly AppDbContext _context;

    public IdempotencyService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> EventHasProcessed(Guid eventId, string handlerName, CancellationToken cancellationToken = default)
    {
        return await _context.ProcessedEvents
            .AnyAsync(e => e.EventId == eventId && e.HandlerName == handlerName, cancellationToken);
    }

    public async Task MarkAsProcessed(Guid eventId, string handlerName, CancellationToken cancellationToken = default)
    {
        var processedEvent = ProcessedEvent.Create(eventId, handlerName);
        _context.ProcessedEvents.Add(processedEvent);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
