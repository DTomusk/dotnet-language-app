namespace Core.Application.Commands;

public record CreateSubmissionCommand(Guid UserID, string LanguageCode, string Text);