using Application.Submissions.Interfaces;
using Domain.LanguagePractice.Entities;
using Infrastructure.LanguagePractice;

namespace Infrastructure.Submissions;

public class SubmissionRespository : ISubmissionRepository
{
    private readonly LanguagePracticeDbContext _context;

    public SubmissionRespository(LanguagePracticeDbContext context)
    {
        _context = context;
    }

    public async Task<Submission> CreateAsync(Submission submission, CancellationToken cancellationToken = default)
    {
        _context.Submissions.Add(submission);
        return submission;
    }
}
