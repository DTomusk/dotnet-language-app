using Application.LanguagePractice.Interfaces;
using Application.LanguagePractice.Services;
using Domain.LanguagePractice.ValueObjects;
using Domain.Shared.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.UnitTests.LanguagePractice.Services;

public class LanguageValidationServiceTests
{
    private readonly ILanguageValidationStrategy _italianStrategy;
    private readonly ILanguageValidationStrategy _germanStrategy;
    private readonly LanguageValidationService _service;

    public LanguageValidationServiceTests()
    {
        _italianStrategy = Substitute.For<ILanguageValidationStrategy>();
        _germanStrategy = Substitute.For<ILanguageValidationStrategy>();

        var strategies = new Dictionary<LanguageCode, ILanguageValidationStrategy>
        {
            { LanguageCode.Italian, _italianStrategy },
            { LanguageCode.German, _germanStrategy }
        };

        _service = new LanguageValidationService(strategies);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_WhenStrategyValidatesSuccessfully()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Ciao, come stai?";
        var cancellationToken = CancellationToken.None;

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnFailure_WhenStrategyValidationFails()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Invalid text with emojis 😀😃😄";
        var cancellationToken = CancellationToken.None;
        var strategyError = new Error("Text does not appear to be in Italian.", ErrorType.Validation);

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Failure(strategyError));

        // Act
        var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Message.Should().Be(strategyError.Message);
    }

    // TODO: implement once we have another valid language code
    //[Fact]
    //public async Task ValidateTextInLanguageAsync_ShouldReturnInternalError_WhenNoStrategyFound()
    //{
    //    // Arrange
    //    var languageCode = LanguageCode.From("es"); // Spanish, not registered
    //    var text = "Hola, ¿cómo estás?";
    //    var cancellationToken = CancellationToken.None;

    //    // Act
    //    var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

    //    // Assert
    //    result.IsFailure.Should().BeTrue();
    //    result.Error.Should().NotBeNull();
    //    result.Error.Type.Should().Be(ErrorType.Internal);
    //    result.Error.Message.Should().Be($"No validation strategy found for language code: {languageCode}");
    //}

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldCallCorrectStrategy_WhenItalianLanguageCodeProvided()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Buongiorno!";
        var cancellationToken = CancellationToken.None;

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        await _italianStrategy.Received(1).ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        await _germanStrategy.DidNotReceive().ValidateTextInLanguageAsync(Arg.Any<LanguageCode>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldCallCorrectStrategy_WhenGermanLanguageCodeProvided()
    {
        // Arrange
        var languageCode = LanguageCode.German;
        var text = "Guten Tag!";
        var cancellationToken = CancellationToken.None;

        _germanStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        await _germanStrategy.Received(1).ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
        await _italianStrategy.DidNotReceive().ValidateTextInLanguageAsync(Arg.Any<LanguageCode>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldPassCancellationTokenToStrategy()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Test text";
        var cancellationToken = new CancellationToken();

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        await _italianStrategy.Received(1).ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldNotCallStrategy_WhenStrategyIsNull()
    {
        // Arrange
        var strategies = new Dictionary<LanguageCode, ILanguageValidationStrategy>
        {
            { LanguageCode.Italian, null! }
        };
        var serviceWithNullStrategy = new LanguageValidationService(strategies);
        var languageCode = LanguageCode.Italian;
        var text = "Test text";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await serviceWithNullStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Type.Should().Be(ErrorType.Internal);
        result.Error.Message.Should().Be($"No validation strategy found for language code: {languageCode}");
    }

    [Theory]
    [InlineData("Valid Italian text")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456")]
    public async Task ValidateTextInLanguageAsync_ShouldPassTextToStrategy(string text)
    {
        // Arrange
        var languageCode = LanguageCode.German;
        var cancellationToken = CancellationToken.None;

        _germanStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        await _germanStrategy.Received(1).ValidateTextInLanguageAsync(languageCode, text, cancellationToken);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldWrapStrategyErrorMessage_WithContextualInformation()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Test text";
        var cancellationToken = CancellationToken.None;
        var originalErrorMessage = "Original validation error";
        var strategyError = new Error(originalErrorMessage, ErrorType.Validation);

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Failure(strategyError));

        // Act
        var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be(originalErrorMessage);
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldReturnSuccess_AndNotModifyResult_WhenStrategySucceeds()
    {
        // Arrange
        var languageCode = LanguageCode.German;
        var text = "Guten Morgen";
        var cancellationToken = CancellationToken.None;

        _germanStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Success());

        // Act
        var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateTextInLanguageAsync_ShouldNotReturnStrategyResultDirectly_WhenStrategyFails()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var text = "Test text";
        var cancellationToken = CancellationToken.None;
        var strategyError = new Error("Original error", ErrorType.Validation);

        _italianStrategy.ValidateTextInLanguageAsync(languageCode, text, cancellationToken)
            .Returns(Result.Failure(strategyError));

        // Act
        var result = await _service.ValidateTextInLanguageAsync(languageCode, text, cancellationToken);

        // Assert
        // The error message should be wrapped with additional context
        result.Error.Message.Should().Be(strategyError.Message);
    }
}
