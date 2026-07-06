using Core.Application.Interfaces;
using Core.Domain.Entities;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
