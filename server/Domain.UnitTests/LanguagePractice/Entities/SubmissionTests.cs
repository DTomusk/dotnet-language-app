using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.LanguagePractice.Entities;

public class SubmissionTests
{
    [Fact]
    public void Create_Should_Create_Submission_With_Valid_Properties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;
        var text = "Ciao, come stai?";

        // Act
        var submission = Submission.Create(userId, languageCode, text);

        // Assert
        submission.Id.Should().NotBe(Guid.Empty);
        submission.UserId.Should().Be(userId);
        submission.LanguageCode.Should().Be(languageCode);
        submission.Text.Should().Be(text);
        submission.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Create_Should_Throw_When_Text_Is_Invalid(string? invalidText)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;

        // Act
        var act = () => Submission.Create(userId, languageCode, invalidText!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Text cannot be empty or whitespace.*")
            .WithParameterName("text");
    }

    [Fact]
    public void Create_Should_Throw_When_UserId_Is_Empty()
    {
        // Arrange
        var userId = Guid.Empty;
        var languageCode = LanguageCode.Italian;
        var text = "Valid text";

        // Act
        var act = () => Submission.Create(userId, languageCode, text);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("UserId cannot be empty.*")
            .WithParameterName("userId");
    }

    [Fact]
    public void Create_Should_Throw_When_LanguageCode_Is_Null()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var text = "Valid text";

        // Act
        var act = () => Submission.Create(userId, null!, text);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("languageCode");
    }

    [Fact]
    public void Create_Should_Generate_Unique_Id_For_Each_Submission()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;

        // Act
        var submission1 = Submission.Create(userId, languageCode, "Text 1");
        var submission2 = Submission.Create(userId, languageCode, "Text 2");

        // Assert
        submission1.Id.Should().NotBe(submission2.Id);
        submission1.Id.Should().NotBe(Guid.Empty);
        submission2.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_Should_Allow_Different_Users_To_Submit_Same_Text()
    {
        // Arrange
        var text = "Buongiorno";
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;

        // Act
        var submission1 = Submission.Create(user1Id, languageCode, text);
        var submission2 = Submission.Create(user2Id, languageCode, text);

        // Assert
        submission1.Id.Should().NotBe(submission2.Id);
        submission1.UserId.Should().NotBe(submission2.UserId);
        submission1.Text.Should().Be(submission2.Text);
    }

    [Fact]
    public void Create_Should_Support_Long_Text()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;
        var longText = new string('a', 10000);

        // Act
        var submission = Submission.Create(userId, languageCode, longText);

        // Assert
        submission.Text.Should().HaveLength(10000);
        submission.Text.Should().Be(longText);
    }

    [Fact]
    public void Create_Should_Support_Unicode_Characters()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;
        var unicodeText = "àèéìòù 你好 مرحبا 🍕";

        // Act
        var submission = Submission.Create(userId, languageCode, unicodeText);

        // Assert
        submission.Text.Should().Be(unicodeText);
    }

    [Fact]
    public void Create_Should_Support_Multiline_Text()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var languageCode = LanguageCode.Italian;
        var multilineText = @"Prima riga
Seconda riga
Terza riga";

        // Act
        var submission = Submission.Create(userId, languageCode, multilineText);

        // Assert
        submission.Text.Should().Contain("Prima riga");
        submission.Text.Should().Contain("Seconda riga");
        submission.Text.Should().Contain("Terza riga");
    }

    [Fact]
    public void UpdateText_Should_Update_Text()
    {
        // Arrange
        var submission = Submission.Create(Guid.NewGuid(), LanguageCode.Italian, "Original text");
        var newText = "Updated text";

        // Act
        submission.UpdateText(newText);

        // Assert
        submission.Text.Should().Be(newText);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void UpdateText_Should_Throw_When_Text_Is_Invalid(string? invalidText)
    {
        // Arrange
        var submission = Submission.Create(Guid.NewGuid(), LanguageCode.Italian, "Original text");

        // Act
        var act = () => submission.UpdateText(invalidText!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Text cannot be empty or whitespace.*")
            .WithParameterName("newText");
    }

    [Fact]
    public void Submission_Properties_Should_Have_Private_Setters()
    {
        // Arrange & Act
        var idProperty = typeof(Submission).GetProperty(nameof(Submission.Id));
        var userIdProperty = typeof(Submission).GetProperty(nameof(Submission.UserId));
        var languageCodeProperty = typeof(Submission).GetProperty(nameof(Submission.LanguageCode));
        var textProperty = typeof(Submission).GetProperty(nameof(Submission.Text));
        var createdAtProperty = typeof(Submission).GetProperty(nameof(Submission.CreatedAt));

        // Assert
        idProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        userIdProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        languageCodeProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        textProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        createdAtProperty!.SetMethod!.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreatedAt_Should_Be_Set_To_UtcNow_When_Submission_Is_Created()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var submission = Submission.Create(Guid.NewGuid(), LanguageCode.Italian, "Test text");

        // Assert
        var afterCreation = DateTime.UtcNow;
        submission.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        submission.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}
