using Application.Auth.Interfaces;
using Infrastructure.Shared;

namespace Infrastructure.Auth;

public class AuthUnitOfWork : EfUnitOfWork<AuthDbContext>, IAuthUnitOfWork
{
    public AuthUnitOfWork(AuthDbContext context) : base(context)
    {
    }
}
