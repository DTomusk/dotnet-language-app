namespace Application.Shared.Interfaces;

public interface IIdempotencyService
{
    Task<bool> EventHasProcessed(Guid eventId, string handlerName, CancellationToken cancellationToken = default);
    Task MarkAsProcessed(Guid eventId, string handlerName, CancellationToken cancellationToken = default);
}
