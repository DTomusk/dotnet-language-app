namespace Application.LanguagePractice.DTOs;

public record LanguageStatsResponse(string DisplayName, int UniqueLemmas, int DaysPractised);