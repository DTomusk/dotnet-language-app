using Domain.LanguagePractice.ValueObjects;

namespace Domain.LanguagePractice.Entities;

public class LanguageLearner
{
    private LanguageLearner() { } // For EF Core

    public Guid UserId { get; private set; }
    public LanguageCode? ActiveLanguage { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static LanguageLearner Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        return new LanguageLearner
        {
            UserId = userId,
            ActiveLanguage = null,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetActiveLanguage(LanguageCode languageCode)
    {
        if (languageCode == null)
            throw new ArgumentNullException(nameof(languageCode));

        ActiveLanguage = languageCode;
    }

    public void ClearActiveLanguage()
    {
        ActiveLanguage = null;
    }
}
