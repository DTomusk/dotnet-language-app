using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;
using Infrastructure.LanguagePractice.Strategies;

namespace Infrastructure.UnitTests.LanguagePractice.Strategies;

public class ItalianValidationStrategyTests
{
    private readonly ItalianValidationStrategy _strategy = new();

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextIsValidItalian()
    {
        // Arrange
        var text = "Ciao, come stai? Io sto bene, grazie.";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsItalianAccents()
    {
        // Arrange
        var text = "L'università è molto bella. C'è una grande biblioteca.";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsNumbers()
    {
        // Arrange
        var text = "Ci sono 42 studenti nella classe. Il numero è 123456.";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsAllowedPunctuation()
    {
        // Arrange
        var text = "Domanda: come vai? Risposta: bene! (Grazie mille.)";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextIsEmpty()
    {
        // Arrange
        var text = "";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
        Assert.Contains("empty", result.Error?.Message ?? "", StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextIsWhitespace()
    {
        // Arrange
        var text = "   \t\n  ";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextIsNotItalian()
    {
        // Arrange
        // Use Cyrillic text which is not in the Italian character set
        var text = "Привет, это русский текст и должен не пройти проверку.";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
        Assert.Contains("Italian", result.Error?.Message ?? "", StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextContainsTooManyDisallowedCharacters()
    {
        // Arrange
        // Mix of Italian with Chinese characters should fail (too many disallowed chars)
        var text = "Ciao 你好 世界 Grazie";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextContainsTooManyEmoji()
    {
        // Arrange
        var text = "Ciao 😀😃😄😁 Questo è testo italiano";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextHasCapitalLetters()
    {
        // Arrange
        var text = "ITALIANO è UNA BELLA LINGUA";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextIsMixedCase()
    {
        // Arrange
        var text = "ItAlIaN è UnA bElLa LiNgUa";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextIsLatinAlphabet()
    {
        // Arrange
        var text = "Even though this is English, the validation should pass because it is in the Latin alphabet.";
        var languageCode = LanguageCode.Italian;
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        // Assert
        Assert.True(result.IsSuccess);
    }
}
