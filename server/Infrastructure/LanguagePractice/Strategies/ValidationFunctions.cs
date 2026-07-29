namespace Infrastructure.LanguagePractice.Strategies;

public static class ValidationFunctions
{
    public static bool IsValidText(string text, double acceptableRatio, HashSet<char> validCharacters)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var validCharCount = text.Count(c => validCharacters.Contains(c));
        var ratio = (double)validCharCount / text.Length;

        return ratio >= acceptableRatio;
    }
}
