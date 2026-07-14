using Application.Submissions.Interfaces;
using Domain.LanguagePractice.Entities;
using Infrastructure.Shared;

namespace Infrastructure.LanguagePractice.Repositories;

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
