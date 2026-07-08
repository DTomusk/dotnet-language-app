namespace Application.Auth.Interfaces;
public interface IAuthUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
