using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Domain.LanguagePractice.Entities;

public class LanguageAnalysis
{
    private LanguageAnalysis() { }

    public Guid Id { get; init; }
    public Guid SubmissionId { get; init; }
    public Guid UserId { get; init; }
    public required LanguageCode LanguageCode { get; init; }
    public LanguageAnalysisStatus Status { get; private set; }
    public DateTime StartedAt { get; init; }
    public DateTime? CompletedAt { get; private set; }
    public IEnumerable<Lemma> Lemmas { get; private set; } = new List<Lemma>();

    public static Result<LanguageAnalysis> Create(Guid submissionId, Guid userId, LanguageCode language)
    {
        var analysis = new LanguageAnalysis
        {
            Id = Guid.NewGuid(),
            SubmissionId = submissionId,
            UserId = userId,
            LanguageCode = language,
            Status = LanguageAnalysisStatus.Pending,
            StartedAt = DateTime.UtcNow
        };

        return Result<LanguageAnalysis>.Success(analysis);
    }

    public void MarkAsCompleted()
    {
        Status = LanguageAnalysisStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = LanguageAnalysisStatus.Failed;
        CompletedAt = DateTime.UtcNow;
    }

    public void AddLemmas(IEnumerable<Lemma> lemmas)
    {
        Lemmas = Lemmas.Concat(lemmas);
    }
}

// TODO: we may want this to be a shared status enum? 
// TODO: consider in progress status
public enum LanguageAnalysisStatus
{
    Pending,
    Completed,
    Failed
}