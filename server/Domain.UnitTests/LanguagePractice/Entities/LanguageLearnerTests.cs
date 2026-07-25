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

    [Fact]
    public void Create_Should_Initialize_Empty_LemmaStatistics_Collection()
    {
        // Arrange & Act
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;

        // Assert
        learner.LemmaStatistics.Should().NotBeNull();
        learner.LemmaStatistics.Should().BeEmpty();
    }

    [Fact]
    public void Create_Should_Initialize_Empty_LanguageStats_Collection()
    {
        // Arrange & Act
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;

        // Assert
        learner.LanguageStats.Should().NotBeNull();
        learner.LanguageStats.Should().BeEmpty();
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Return_Success_When_NewLemmas_Is_Empty()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;

        // Act
        var result = learner.UpdateLemmaStatistics(new List<Lemma>(), languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LemmaStatistics.Should().BeEmpty();
        learner.LanguageStats.Should().BeEmpty();
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Return_Success_When_NewLemmas_Is_Null()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;

        // Act
        var result = learner.UpdateLemmaStatistics(null!, languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LemmaStatistics.Should().BeEmpty();
        learner.LanguageStats.Should().BeEmpty();
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Add_New_Lemma_Statistics()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao"), new Lemma("buongiorno") };

        // Act
        var result = learner.UpdateLemmaStatistics(lemmas, languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LemmaStatistics.Should().HaveCount(2);
        learner.LemmaStatistics.Should().Contain(ls => ls.Text == "ciao" && ls.Frequency == 1);
        learner.LemmaStatistics.Should().Contain(ls => ls.Text == "buongiorno" && ls.Frequency == 1);
        learner.LemmaStatistics.Should().AllSatisfy(ls => ls.LanguageCode.Should().Be("it"));
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Increment_Frequency_For_Existing_Lemma()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao") };

        learner.UpdateLemmaStatistics(lemmas, languageCode);
        var firstStat = learner.LemmaStatistics.First();

        // Act
        var result = learner.UpdateLemmaStatistics(lemmas, languageCode);
        var updatedStat = learner.LemmaStatistics.First();

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LemmaStatistics.Should().HaveCount(1);
        updatedStat.Frequency.Should().Be(2);
        updatedStat.LastUsedAt.Should().BeOnOrAfter(firstStat.LastUsedAt);
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Record_Submission_Statistics()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var learnerResult = LanguageLearner.Create(userId);
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao"), new Lemma("buongiorno") };

        // Act
        var result = learner.UpdateLemmaStatistics(lemmas, languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LanguageStats.Should().HaveCount(1);
        var stat = learner.LanguageStats.First();
        stat.LanguageCode.Should().Be(languageCode);
        stat.TotalSubmissions.Should().Be(1);
        stat.UniqueLemmas.Should().Be(2);
        stat.LanguageLearnerId.Should().Be(userId);
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Increment_TotalSubmissions_For_Existing_Language()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var firstLemmas = new[] { new Lemma("ciao") };
        var secondLemmas = new[] { new Lemma("arrivederci") };

        learner.UpdateLemmaStatistics(firstLemmas, languageCode);

        // Act
        var result = learner.UpdateLemmaStatistics(secondLemmas, languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LanguageStats.Should().HaveCount(1);
        var stat = learner.LanguageStats.First();
        stat.TotalSubmissions.Should().Be(2);
        stat.UniqueLemmas.Should().Be(2);
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Not_Increment_UniqueLemmas_For_Duplicate_Lemma()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var firstLemmas = new[] { new Lemma("ciao") };
        var secondLemmas = new[] { new Lemma("ciao") };

        learner.UpdateLemmaStatistics(firstLemmas, languageCode);

        // Act
        var result = learner.UpdateLemmaStatistics(secondLemmas, languageCode);

        // Assert
        result.IsSuccess.Should().BeTrue();
        learner.LanguageStats.Should().HaveCount(1);
        var stat = learner.LanguageStats.First();
        stat.TotalSubmissions.Should().Be(2);
        stat.UniqueLemmas.Should().Be(1);
    }

    [Fact]
    public void UpdateLemmaStatistics_Should_Track_Same_Language_With_Mixed_Lemmas()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var firstBatch = new[] { new Lemma("ciao"), new Lemma("buongiorno") };
        var secondBatch = new[] { new Lemma("arrivederci"), new Lemma("grazie") };

        // Act
        learner.UpdateLemmaStatistics(firstBatch, languageCode);
        learner.UpdateLemmaStatistics(secondBatch, languageCode);

        // Assert
        learner.LanguageStats.Should().HaveCount(1);
        var stat = learner.LanguageStats.First();
        stat.LanguageCode.Should().Be(languageCode);
        stat.TotalSubmissions.Should().Be(2);
        stat.UniqueLemmas.Should().Be(4);
        learner.LemmaStatistics.Should().HaveCount(4);
    }

    [Fact]
    public void LemmaStatistics_Should_Have_FirstUsedAt_Set_When_Created()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao") };
        var beforeUpdate = DateTime.UtcNow;

        // Act
        learner.UpdateLemmaStatistics(lemmas, languageCode);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        var stat = learner.LemmaStatistics.First();
        stat.FirstUsedAt.Should().BeOnOrAfter(beforeUpdate);
        stat.FirstUsedAt.Should().BeOnOrBefore(afterUpdate);
        stat.LastUsedAt.Should().Be(stat.FirstUsedAt);
    }

    [Fact]
    public void LanguageStats_Should_Have_StartedLearningAt_Set_When_Created()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao") };
        var beforeUpdate = DateTime.UtcNow;

        // Act
        learner.UpdateLemmaStatistics(lemmas, languageCode);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        var stat = learner.LanguageStats.First();
        stat.StartedLearningAt.Should().BeOnOrAfter(beforeUpdate);
        stat.StartedLearningAt.Should().BeOnOrBefore(afterUpdate);
        stat.LastSubmissionAt.Should().BeOnOrAfter(beforeUpdate);
        stat.LastSubmissionAt.Should().BeOnOrBefore(afterUpdate);
    }

    [Fact]
    public void LanguageStats_Should_Update_LastSubmissionAt_On_Subsequent_Submission()
    {
        // Arrange
        var learnerResult = LanguageLearner.Create(Guid.NewGuid());
        var learner = learnerResult.Value;
        var languageCode = LanguageCode.Italian;
        var lemmas = new[] { new Lemma("ciao") };

        learner.UpdateLemmaStatistics(lemmas, languageCode);
        var firstStat = learner.LanguageStats.First();
        var delayMs = 100;
        System.Threading.Thread.Sleep(delayMs);

        // Act
        learner.UpdateLemmaStatistics(lemmas, languageCode);
        var updatedStat = learner.LanguageStats.First();

        // Assert
        updatedStat.LastSubmissionAt.Should().BeAfter(firstStat.LastSubmissionAt!.Value);
    }
}
