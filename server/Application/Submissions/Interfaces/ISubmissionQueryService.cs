using Application.Submissions.DTOs;

namespace Application.Submissions.Interfaces;

public interface ISubmissionQueryService
{
    Task<IEnumerable<SubmissionResponse>> GetSubmissionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<SubmissionResponse?> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
