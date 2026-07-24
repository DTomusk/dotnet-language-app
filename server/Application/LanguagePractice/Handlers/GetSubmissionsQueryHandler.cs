using Application.Shared.Interfaces;
using Application.Submissions.DTOs;
using Application.Submissions.Interfaces;
using Application.Submissions.Queries;

namespace Application.Submissions.Handlers;

public class GetSubmissionsQueryHandler : IQueryHandler<GetSubmissionsQuery, IEnumerable<SubmissionResponse>>
{
    private readonly ISubmissionQueryService _submissionQueryService;

    public GetSubmissionsQueryHandler(ISubmissionQueryService submissionQueryService)
    {
        _submissionQueryService = submissionQueryService;
    }

    public Task<IEnumerable<SubmissionResponse>> HandleAsync(GetSubmissionsQuery command, CancellationToken cancellationToken = default)
    {
        return _submissionQueryService.GetSubmissionsByUserIdAsync(command.UserId, cancellationToken);
    }
}
