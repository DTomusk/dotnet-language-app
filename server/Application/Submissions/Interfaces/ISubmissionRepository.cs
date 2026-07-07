using Domain.Entities;

namespace Application.Submissions.Interfaces;

public interface ISubmissionRepository
{
    Task<IEnumerable<Submission>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Submission> CreateAsync(Submission submission, CancellationToken cancellationToken = default);
}
