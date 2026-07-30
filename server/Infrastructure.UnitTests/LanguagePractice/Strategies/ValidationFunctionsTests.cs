using Infrastructure.LanguagePractice.Strategies;

namespace Infrastructure.UnitTests.LanguagePractice.Strategies;

public class ValidationFunctionsTests
{
    public static IEnumerable<object[]> ValidTextTestData =>
        new List<object[]>
        {
            new object[] { "!!!", 1, new HashSet<char> { '!', 'a', } },
            new object[] { "aaab", 0.6, new HashSet<char> { 'a' } },
            // Edge case: 100% valid characters
            new object[] { "abc", 1.0, new HashSet<char> { 'a', 'b', 'c' } },
            // Edge case: 0% acceptable ratio
            new object[] { "xyz", 0.0, new HashSet<char> { 'x' } },
            // Boundary: exactly at ratio threshold
            new object[] { "aabb", 0.5, new HashSet<char> { 'a' } },
            // Single character
            new object[] { "a", 1.0, new HashSet<char> { 'a' } },
            // Numbers and special characters
            new object[] { "a1b2c3", 0.5, new HashSet<char> { 'a', 'b', 'c' } },
            // Mixed valid and invalid with low threshold
            new object[] { "aabbcc", 0.33, new HashSet<char> { 'a', 'b' } },
            // Case-sensitive: uppercase letters
            new object[] { "AaBbCc", 0.5, new HashSet<char> { 'A', 'B', 'C' } },
        };

    public static IEnumerable<object[]> InvalidTextTestData =>
        new List<object[]>
        {
            new object[] { "!!!", 1, new HashSet<char> { 'a', 'b' } },
            new object[] { "aaab", 0.8, new HashSet<char> { 'a' } },
            new object[] { "", 0.5, new HashSet<char> { 'a' } },
            new object[] { "   ", 0.5, new HashSet<char> { 'a' } },
            // High ratio requirement with mostly invalid characters
            new object[] { "axxxxx", 0.9, new HashSet<char> { 'a' } },
            // Just below threshold
            new object[] { "aab", 0.75, new HashSet<char> { 'a' } },
            // No valid characters
            new object[] { "xyz123", 0.1, new HashSet<char> { 'a', 'b' } },
            // Single character doesn't match
            new object[] { "x", 1.0, new HashSet<char> { 'a', 'b' } },
            // Tab and newline (whitespace)
            new object[] { "\t\n", 0.5, new HashSet<char> { 'a' } },
        };

    [Theory]
    [MemberData(nameof(ValidTextTestData))]
    public void IsValidText_ShouldReturnTrue_WhenTextIsValid(string text, double acceptableRatio, HashSet<char> validCharacters)
    {
        // Act
        var result = ValidationFunctions.IsValidText(text, acceptableRatio, validCharacters);
        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(InvalidTextTestData))]
    public void IsValidText_ShouldReturnFalse_WhenTextIsInvalid(string text, double acceptableRatio, HashSet<char> validCharacters)
    {
        // Act
        var result = ValidationFunctions.IsValidText(text, acceptableRatio, validCharacters);
        // Assert
        Assert.False(result);
    }
}