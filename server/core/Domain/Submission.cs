namespace Core.Domain;

public record Submission(Guid ID, Guid UserID, string LanguageCode, string Text);
