namespace Application.Shared.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
