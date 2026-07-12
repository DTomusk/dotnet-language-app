using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Domain.LanguagePractice.Entities;

public class LanguageLearner
{
    private LanguageLearner() { } // For EF Core

    public Guid UserId { get; private set; }
    public LanguageCode? ActiveLanguage { get; private set; }
    public DateTime CreatedAt { get; private set; }

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
}
