namespace Application.LanguagePractice.Commands;

public record SetPracticeLanguageCommand(Guid UserId, string LanguageCode);