using Domain.LanguagePractice.ValueObjects;

namespace Domain.LanguagePractice.Entities;

public class Submission
{
    private string _text = string.Empty;

    private Submission() { } // For EF Core

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public LanguageCode LanguageCode { get; private set; } = null!;

    public string Text
    {
        get => _text;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Text cannot be empty or whitespace.", nameof(Text));

            _text = value;
        }
    }

    public DateTime CreatedAt { get; private set; }

    public static Submission Create(Guid userId, LanguageCode languageCode, string text)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        if (languageCode == null)
            throw new ArgumentNullException(nameof(languageCode));

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(text));

        return new Submission
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LanguageCode = languageCode,
            Text = text,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(newText));

        Text = newText;
    }
}
