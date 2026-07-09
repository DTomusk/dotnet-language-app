using Application.Shared.Interfaces;
using Domain.Shared.Events;
using System.Text.Json;

namespace Infrastructure.Shared;

public class DomainEventPublisher : IDomainEventPublisher
{
    public readonly AppDbContext _context;

    public DomainEventPublisher(AppDbContext context)
    {
        _context = context;
    }  

    public Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var serializedPayload = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
        var outboxMessage = OutboxMessage.Create(domainEvent, serializedPayload);

        _context.OutboxMessages.Add(outboxMessage);

        return Task.CompletedTask;
    }
}
