using Application.Auth.Interfaces;
using Domain.Auth.Entities;
using Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        return user;
    }

    public async Task<User?> GetByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.DisplayName == displayName, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FindAsync([id], cancellationToken);
    }
}
