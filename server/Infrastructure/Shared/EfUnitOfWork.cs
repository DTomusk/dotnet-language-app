using Application.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared;

public class EfUnitOfWork<TContext> where TContext : DbContext
{
    private readonly TContext _context;

    public EfUnitOfWork(TContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
