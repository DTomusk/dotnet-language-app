using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;
using Infrastructure.LanguagePractice.Strategies;
using Xunit;

namespace Infrastructure.UnitTests.LanguagePractice.Strategies;

public class GermanValidationStrategyTests
{
    private readonly GermanValidationStrategy _strategy = new();

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextIsValidGerman()
    {
        // Arrange
        var text = "Guten Tag! Wie geht es dir heute?";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsGermanUmlauts()
    {
        // Arrange
        var text = "Die Schöne Müller trägt einen schönen Hut.";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsCapitalUmlauts()
    {
        // Arrange
        var text = "Der Arzt, die Ärztin und der Ärzte.";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsEszett()
    {
        // Arrange
        var text = "Das ist eine große Straße in München.";
        var languageCode = LanguageCode.German;
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
        var text = "Es gibt 100 Studenten. Die Nummer ist 42.";
        var languageCode = LanguageCode.German;
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
        var text = "Frage: Wie heißt du? Antwort: Mein Name ist Max! (Es freut mich.)";
        var languageCode = LanguageCode.German;
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
        var languageCode = LanguageCode.German;
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
        var text = "\t  \n  ";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextIsNotGerman()
    {
        // Arrange
        // Use Arabic text which is not in the German character set
        var text = "هذا نص عربي ويجب أن يفشل التحقق.";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
        Assert.Contains("German", result.Error?.Message ?? "", StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenTextContainsTooManyDisallowedCharacters()
    {
        // Arrange
        // Mix of German with Greek characters should fail
        var text = "Guten Tag αβγδ εζηθ Danke schön";
        var languageCode = LanguageCode.German;
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
        var text = "Hallo 🎉🎊🎈🎁 Das ist Deutsch";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextHasAllCapitalLetters()
    {
        // Arrange
        var text = "DEUTSCH IST EINE SCHÖNE SPRACHE";
        var languageCode = LanguageCode.German;
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
        var text = "DeUtScH iSt EiNe ScHöNe SpRaChE";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenTextContainsLineBreaks()
    {
        // Arrange
        var text = "Das ist die erste Zeile.\nDas ist die zweite Zeile.";
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _strategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
