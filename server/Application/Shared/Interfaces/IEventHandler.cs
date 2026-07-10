using Domain.Shared.Events;

namespace Application.Shared.Interfaces;

// in means we can pass types derived from T, but we cannot return T or use it as a return type
public interface IEventHandler<in TEvent> where TEvent: DomainEvent
{
    // @ simply escapes event, as event is a reserved keyword in C#
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
