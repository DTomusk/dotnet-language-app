using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LanguagePractice.ValueObjects;

/// <summary>
/// Represents aggregated stats for a language learner in the context of a language
/// </summary>
public record LanguageLearnerStats
{
    public LanguageCode LanguageCode { get; init; }
    public int TotalSubmissions { get; init; } = 0;
    public int UniqueLemmas { get; init; } = 0;
    public DateTime StartedLearningAt { get; init; }
    public DateTime? LastSubmissionAt { get; init; }

    private LanguageLearnerStats() { }

    public LanguageLearnerStats(LanguageCode languageCode, int totalSubmissions, int uniqueLemmas, DateTime startedLearningAt)
    {
        LanguageCode = languageCode;
        TotalSubmissions = totalSubmissions;
        UniqueLemmas = uniqueLemmas;
        StartedLearningAt = startedLearningAt;
        LastSubmissionAt = startedLearningAt;
    }
}
