using Domain.LanguagePractice.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.LanguagePractice.ValueObjects;

public class LanguageCodeTests
{
    [Fact]
    public void Italian_Should_Return_Italian_LanguageCode()
    {
        // Act
        var languageCode = LanguageCode.Italian;

        // Assert
        languageCode.Value.Should().Be("it");
    }

    [Theory]
    [InlineData("it")]
    [InlineData("IT")]
    [InlineData("It")]
    [InlineData("iT")]
    public void From_Should_Return_Italian_For_Valid_Italian_Codes(string code)
    {
        // Act
        var languageCode = LanguageCode.From(code);

        // Assert
        languageCode.Should().Be(LanguageCode.Italian);
        languageCode.Value.Should().Be("it");
    }

    [Theory]
    [InlineData("en")]
    [InlineData("fr")]
    [InlineData("es")]
    [InlineData("de")]
    [InlineData("invalid")]
    [InlineData("")]
    public void From_Should_Throw_ArgumentException_For_Unsupported_Language_Codes(string invalidCode)
    {
        // Act
        var act = () => LanguageCode.From(invalidCode);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid language code: {invalidCode}");
    }

    [Fact]
    public void From_Should_Throw_For_Null_Code()
    {
        // Act
        var act = () => LanguageCode.From(null!);

        // Assert
        act.Should().Throw<Exception>("null input should throw an exception");
    }

    [Fact]
    public void ToString_Should_Return_Value()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;

        // Act
        var result = languageCode.ToString();

        // Assert
        result.Should().Be("it");
    }

    [Fact]
    public void LanguageCode_Should_Support_Equality_Comparison()
    {
        // Arrange
        var code1 = LanguageCode.Italian;
        var code2 = LanguageCode.From("it");
        var code3 = LanguageCode.From("IT");

        // Assert
        code1.Should().Be(code2);
        code1.Should().Be(code3);
        code2.Should().Be(code3);
        (code1 == code2).Should().BeTrue();
        (code1 == code3).Should().BeTrue();
    }

    [Fact]
    public void LanguageCode_Should_Support_Inequality_Comparison()
    {
        // Arrange
        var italian = LanguageCode.Italian;

        // Act & Assert
        // Since we only have Italian, we can't compare with different languages
        // But we can test that the same language is equal
        (italian != LanguageCode.Italian).Should().BeFalse();
    }

    [Fact]
    public void LanguageCode_Should_Have_Same_HashCode_For_Equal_Values()
    {
        // Arrange
        var code1 = LanguageCode.Italian;
        var code2 = LanguageCode.From("it");

        // Assert
        code1.GetHashCode().Should().Be(code2.GetHashCode());
    }

    [Fact]
    public void LanguageCode_Should_Be_Immutable()
    {
        // Arrange
        var languageCode = LanguageCode.Italian;
        var originalValue = languageCode.Value;

        // Act
        // Value object should be immutable - Value property should have no setter
        var valueProperty = typeof(LanguageCode).GetProperty(nameof(LanguageCode.Value));

        // Assert
        valueProperty!.SetMethod.Should().BeNull("Value should be read-only");
        languageCode.Value.Should().Be(originalValue);
    }

    [Fact]
    public void LanguageCode_Should_Be_Case_Insensitive()
    {
        // Arrange & Act
        var lower = LanguageCode.From("it");
        var upper = LanguageCode.From("IT");
        var mixed1 = LanguageCode.From("It");
        var mixed2 = LanguageCode.From("iT");

        // Assert
        lower.Should().Be(upper);
        lower.Should().Be(mixed1);
        lower.Should().Be(mixed2);
        lower.Value.Should().Be("it", "should always normalize to lowercase");
    }
}
