using Application.Shared.Interfaces;
using Application.Submissions.DTOs;
using Application.Submissions.Queries;

namespace Application.Submissions.Handlers;

public class GetSubmissionsQueryHandler : ICommandHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>>
{
    public Task<IEnumerable<SubmissionResponse>> HandleAsync(GetSubmissionsQuery command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
