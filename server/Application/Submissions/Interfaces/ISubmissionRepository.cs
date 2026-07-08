using Domain.Entities;

namespace Application.Submissions.Interfaces;

public interface ISubmissionRepository
{
    Task<Submission> CreateAsync(Submission submission, CancellationToken cancellationToken = default);
}
