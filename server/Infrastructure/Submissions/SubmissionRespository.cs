using Application.Submissions.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Submissions;

public class SubmissionRespository : ISubmissionRepository
{
    private readonly AppDbContext _context;

    public SubmissionRespository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Submission> CreateAsync(Submission submission, CancellationToken cancellationToken = default)
    {
        _context.Submissions.Add(submission);
        return submission;
    }
}
