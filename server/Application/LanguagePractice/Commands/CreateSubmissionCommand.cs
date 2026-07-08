namespace Application.Submissions.Commands;

public record CreateSubmissionCommand(Guid UserID, string LanguageCode, string Text);