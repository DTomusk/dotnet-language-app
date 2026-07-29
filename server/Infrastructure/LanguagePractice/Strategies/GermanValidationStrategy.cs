using Application.LanguagePractice.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;

namespace Infrastructure.LanguagePractice.Strategies;

public class GermanValidationStrategy : ILanguageValidationStrategy
{
    // Hardcoded list of valid characters in German
    // Doesn't include lots of special characters, emoji, etc.
    private static readonly HashSet<char> GermanCharacters = new()
    {
        // Basic Latin
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
        // German-specific
        'ä','ö','ü','ß','Ä','Ö','Ü',
        // Digits and common punctuation
        '0','1','2','3','4','5','6','7','8','9',
        ' ', '.', ',', '!', '?', '-', '"', '\'', ':', ';', '(', ')', '\n', '\r', '\t'
    };

    // TODO: move to configuration (or db) so it's easier to control and adjust
    private const double AcceptableCharacterRatio = 0.95;

    public Task<Result> ValidateTextInLanguageAsync(LanguageCode languageCode, string text, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(Result.Failure(new Error("Text cannot be empty", ErrorType.Validation)));

        if (!ValidationFunctions.IsValidText(text, AcceptableCharacterRatio, GermanCharacters))
            return Task.FromResult(Result.Failure(new Error(
                $"Text does not appear to be in German.",
                ErrorType.Validation)));

        return Task.FromResult(Result.Success());
    }
}