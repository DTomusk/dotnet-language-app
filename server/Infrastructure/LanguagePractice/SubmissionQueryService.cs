using Application.Submissions.DTOs;
using Application.Submissions.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Submissions;

public class SubmissionQueryService : ISubmissionQueryService
{
    private readonly AppDbContext _context;

    public SubmissionQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SubmissionResponse?> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Where(s => s.ID == id)
            .Select(s => new SubmissionResponse(s.Text, s.LanguageCode))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubmissionResponse>> GetSubmissionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Submissions
            .AsNoTracking()
            .Where(s => s.UserID == userId)
            .Select(s => new SubmissionResponse(s.Text, s.LanguageCode))
            .ToListAsync(cancellationToken);
    }
}
