using Domain.LanguagePractice.ValueObjects;

namespace Application.Submissions.DTOs;

// TODO: return submissions in the context of a language 
// i.e. without languageCode, just text
public record SubmissionResponse(string text, LanguageCode languageCode);