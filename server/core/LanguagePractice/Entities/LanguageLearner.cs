using Domain.LanguagePractice.ValueObjects;

namespace Domain.LanguagePractice.Entities;

public class LanguageLearner
{
    public Guid UserId { get; private set; } // References UserId in auth context
    public LanguageCode? ActiveLanguage { get; private set; }
}
