using Domain.LanguagePractice.Entities;
using Domain.LanguagePractice.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.LanguagePractice.Entities;

public class LanguageLearnerTests
{
    [Fact]
    public void Create_Should_Create_LanguageLearner_With_Valid_Properties()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var learnerResult = LanguageLearner.Create(userId);

        // Assert
        learnerResult.IsSuccess.Should().BeTrue();
        var learner = learnerResult.Value;
        learner.Should().NotBeNull();
        learner.UserId.Should().Be(userId);
        learner.ActiveLanguage.Should().BeNull();
        learner.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_Should_Fail_When_UserId_Is_Empty()
    {
        // Arrange
        var userId = Guid.Empty;

        // Act
        var result = LanguageLearner.Create(userId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Message.Should().Be("UserId cannot be empty.");
    }

    [Fact]
    public void SetActiveLanguage_Should_Set_ActiveLanguage()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;

        // Act
        learner.SetActiveLanguage(languageCode);

        // Assert
        learner.ActiveLanguage.Should().Be(languageCode);
    }

    [Fact]
    public void SetActiveLanguage_Should_Fail_When_LanguageCode_Is_Null()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;

        // Act
        var result = learner.SetActiveLanguage(null!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Message.Should().Be("LanguageCode cannot be null.");
    }

    [Fact]
    public void SetActiveLanguage_Should_Update_Existing_ActiveLanguage()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        learner.SetActiveLanguage(LanguageCode.Italian);
        var newLanguageCode = LanguageCode.From("it");

        // Act
        var result = learner.SetActiveLanguage(newLanguageCode);

        // Assert
        result.IsSuccess.Should().BeTrue(); result.Error.Should().BeNull();  
        learner.ActiveLanguage.Should().Be(newLanguageCode);
    }

    [Fact]
    public void ClearActiveLanguage_Should_Set_ActiveLanguage_To_Null()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        learner.SetActiveLanguage(LanguageCode.Italian);

        // Act
        var result = learner.ClearActiveLanguage();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        learner.ActiveLanguage.Should().BeNull();
    }

    [Fact]
    public void ClearActiveLanguage_Should_Work_When_ActiveLanguage_Is_Already_Null()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;

        // Act
        var result = learner.ClearActiveLanguage();

        // Assert
        result.IsSuccess.Should().BeTrue(); 
        learner.ActiveLanguage.Should().BeNull();
    }

    [Fact]
    public void LanguageLearner_Properties_Should_Have_Private_Setters()
    {
        // Arrange & Act
        var userIdProperty = typeof(LanguageLearner).GetProperty(nameof(LanguageLearner.UserId));
        var activeLanguageProperty = typeof(LanguageLearner).GetProperty(nameof(LanguageLearner.ActiveLanguage));
        var createdAtProperty = typeof(LanguageLearner).GetProperty(nameof(LanguageLearner.CreatedAt));

        // Assert
        userIdProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        activeLanguageProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        createdAtProperty!.SetMethod!.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreatedAt_Should_Be_Set_To_UtcNow_When_LanguageLearner_Is_Created()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;

        // Assert
        var afterCreation = DateTime.UtcNow;
        learner.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        learner.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}
