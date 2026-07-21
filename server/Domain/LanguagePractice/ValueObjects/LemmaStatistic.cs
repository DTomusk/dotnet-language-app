namespace Domain.LanguagePractice.ValueObjects;

public record LemmaStatistic(
    string Text, 
    string LanguageCode,
    int Frequency, 
    DateTime FirstUsedAt, 
    DateTime LastUsedAt
);