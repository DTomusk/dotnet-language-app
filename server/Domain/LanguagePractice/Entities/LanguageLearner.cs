using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Domain.LanguagePractice.Entities;

public class LanguageLearner
{
    private LanguageLearner() { } // For EF Core

    public Guid UserId { get; private set; }
    public LanguageCode? ActiveLanguage { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<LemmaStatistic> LemmaStatistics { get; private set; } = new List<LemmaStatistic>();
    public ICollection<LanguageLearnerStats> LanguageStats { get; private set; } = new List<LanguageLearnerStats>();

    public static Result<LanguageLearner> Create(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result<LanguageLearner>.Failure(new Error("UserId cannot be empty.", ErrorType.Validation));

        return Result<LanguageLearner>.Success(new LanguageLearner
        {
            UserId = userId,
            ActiveLanguage = null,
            CreatedAt = DateTime.UtcNow
        });
    }

    public Result SetActiveLanguage(LanguageCode languageCode)
    {
        if (languageCode == null)
            return Result.Failure(new Error("LanguageCode cannot be null.", ErrorType.Validation));

        ActiveLanguage = languageCode;
        return Result.Success();
    }

    public Result ClearActiveLanguage()
    {
        ActiveLanguage = null;
        return Result.Success();
    }

    public Result UpdateLemmaStatistics(IEnumerable<Lemma> newLemmas, LanguageCode languageCode)
    {
        if (newLemmas == null || !newLemmas.Any())
            return Result.Success();

        var now = DateTime.UtcNow;

        var newLemmaCount = 0;

        foreach (var lemma in newLemmas)
        {
            var existingStat = LemmaStatistics.FirstOrDefault(ls => ls.Text == lemma.Text && ls.LanguageCode == languageCode.ToString());

            if (existingStat != null)
            {
                var updated = existingStat with
                {
                    Frequency = existingStat.Frequency + 1,
                    LastUsedAt = now
                };
                
                // Records are immutable
                LemmaStatistics.Remove(existingStat);
                LemmaStatistics.Add(updated);
            }
            else
            {
                newLemmaCount++;
                var newStat = new LemmaStatistic(
                    lemma.Text,
                    languageCode.ToString(),
                    1,
                    now,
                    now);
                LemmaStatistics.Add(newStat);
            }
        }
        return RecordSubmission(languageCode, newLemmaCount);
    }

    private Result RecordSubmission(LanguageCode languageCode, int newLemmasCount = 0)
    {
        if (languageCode == null)
            return Result.Failure(new Error("LanguageCode cannot be null.", ErrorType.Validation));

        var existingStat = LanguageStats.FirstOrDefault(ls => ls.LanguageCode == languageCode);

        if (existingStat != null)
        {
            var updated = existingStat with
            {
                TotalSubmissions = existingStat.TotalSubmissions + 1,
                UniqueLemmas = existingStat.UniqueLemmas + newLemmasCount,
                LastSubmissionAt = DateTime.UtcNow
            };
            
            LanguageStats.Remove(existingStat);
            LanguageStats.Add(updated);
        }
        else
        {
            var newStat = new LanguageLearnerStats(
                languageCode,
                1,
                newLemmasCount,
                DateTime.UtcNow,
                this.UserId);

            LanguageStats.Add(newStat);
        }

        return Result.Success();
    }
}
