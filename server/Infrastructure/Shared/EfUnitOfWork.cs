using Application.Shared.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Shared;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
