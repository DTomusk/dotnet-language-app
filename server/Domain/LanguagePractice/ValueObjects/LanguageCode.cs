namespace Domain.LanguagePractice.ValueObjects;

public sealed record LanguageCode
{
    public string Value { get; }

    private LanguageCode(string value) => Value = value;

    public static LanguageCode Italian => new("it");
    public static LanguageCode German => new("de");

    public static LanguageCode From(string code)
    {
        return code.ToLowerInvariant() switch
        {
            "it" => Italian,
            "de" => German,
            _ => throw new ArgumentException($"Invalid language code: {code}")
        };
    }

    public static IEnumerable<(string Code, string Name)> GetAllSupportedLanguages()
    {
        yield return ("it", "Italian");
        yield return ("de", "German");
    }

    public override string ToString() => Value;
}
