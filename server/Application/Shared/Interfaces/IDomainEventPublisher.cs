using Domain.Shared.Events;

namespace Application.Shared.Interfaces;

public interface IDomainEventPublisher
{
    Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
