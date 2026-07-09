using Domain.Auth.Entities;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Auth.Entities;

public class UserTests
{
    [Fact]
    public void Create_Should_Create_User_With_Valid_Properties()
    {
        // Arrange
        var displayName = "John Doe";
        var passwordHash = "hashed_password_123";

        // Act
        var user = User.Create(displayName, passwordHash);

        // Assert
        user.Id.Should().NotBe(Guid.Empty);
        user.DisplayName.Should().Be(displayName);
        user.PasswordHash.Should().Be(passwordHash);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void Create_Should_Throw_When_DisplayName_Is_Invalid(string? invalidDisplayName)
    {
        // Arrange
        var passwordHash = "hashed_password_123";

        // Act
        var act = () => User.Create(invalidDisplayName!, passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Display name cannot be empty or whitespace.*")
            .WithParameterName("displayName");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void Create_Should_Throw_When_PasswordHash_Is_Invalid(string? invalidPasswordHash)
    {
        // Arrange
        var displayName = "John Doe";

        // Act
        var act = () => User.Create(displayName, invalidPasswordHash!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Password hash cannot be empty or whitespace.*")
            .WithParameterName("passwordHash");
    }

    [Fact]
    public void Create_Should_Generate_Unique_Id_For_Each_User()
    {
        // Arrange & Act
        var user1 = User.Create("User 1", "hash1");
        var user2 = User.Create("User 2", "hash2");

        // Assert
        user1.Id.Should().NotBe(user2.Id);
        user1.Id.Should().NotBe(Guid.Empty);
        user2.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void UpdateDisplayName_Should_Update_DisplayName()
    {
        // Arrange
        var user = User.Create("Original Name", "hash");
        var newDisplayName = "Updated Name";

        // Act
        user.UpdateDisplayName(newDisplayName);

        // Assert
        user.DisplayName.Should().Be(newDisplayName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void UpdateDisplayName_Should_Throw_When_DisplayName_Is_Invalid(string? invalidDisplayName)
    {
        // Arrange
        var user = User.Create("Original Name", "hash");

        // Act
        var act = () => user.UpdateDisplayName(invalidDisplayName!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Display name cannot be empty or whitespace.*")
            .WithParameterName("newDisplayName");
    }

    [Fact]
    public void UpdatePassword_Should_Update_PasswordHash()
    {
        // Arrange
        var user = User.Create("Test User", "original_hash");
        var newPasswordHash = "new_hash_123";

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void UpdatePassword_Should_Throw_When_PasswordHash_Is_Invalid(string? invalidPasswordHash)
    {
        // Arrange
        var user = User.Create("Test User", "original_hash");

        // Act
        var act = () => user.UpdatePassword(invalidPasswordHash!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Password hash cannot be empty or whitespace.*")
            .WithParameterName("newPasswordHash");
    }

    [Fact]
    public void User_Properties_Should_Have_Private_Setters()
    {
        // Arrange
        var user = User.Create("Test User", "hash");

        // Assert
        var idProperty = typeof(User).GetProperty(nameof(User.Id));
        var passwordHashProperty = typeof(User).GetProperty(nameof(User.PasswordHash));
        var displayNameProperty = typeof(User).GetProperty(nameof(User.DisplayName));
        var createdAtProperty = typeof(User).GetProperty(nameof(User.CreatedAt));

        idProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        passwordHashProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        displayNameProperty!.SetMethod!.IsPrivate.Should().BeTrue();
        createdAtProperty!.SetMethod!.IsPrivate.Should().BeTrue();
    }

    [Fact]
    public void CreatedAt_Should_Be_Set_To_UtcNow_When_User_Is_Created()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var user = User.Create("Test User", "hash");

        // Assert
        var afterCreation = DateTime.UtcNow;
        user.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        user.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}
