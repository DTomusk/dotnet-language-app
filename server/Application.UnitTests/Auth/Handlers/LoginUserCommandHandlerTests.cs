using Application.Auth.Commands;
using Application.Auth.Handlers;
using Application.Auth.Interfaces;
using Domain.Auth.Entities;
using Domain.Shared.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Application.UnitTests.Auth.Handlers;

public class LoginUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _tokenGenerator = Substitute.For<ITokenGenerator>();
        _handler = new LoginUserCommandHandler(_userRepository, _passwordHasher, _tokenGenerator);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Success_With_AuthResponse_When_Credentials_Are_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.Value.Should().NotBeNull();
        result.Value.UserId.Should().Be(user.Id);
        result.Value.DisplayName.Should().Be(displayName);
        result.Value.Token.Should().Be(token);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_User_Not_Found()
    {
        // Arrange
        var displayName = "nonexistentuser";
        var password = "password123";
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Message.Should().Be("Invalid display name or password.");
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Failure_When_Password_Is_Invalid()
    {
        // Arrange
        var displayName = "testuser";
        var password = "wrongpassword";
        var passwordHash = "hashed_password";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(false);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Message.Should().Be("Invalid display name or password.");
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task HandleAsync_Should_Call_GetByDisplayNameAsync_With_Correct_Parameters()
    {
        // Arrange
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        await _userRepository.Received(1).GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_Should_Call_VerifyPassword_With_Correct_Parameters_When_User_Found()
    {
        // Arrange
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _passwordHasher.Received(1).VerifyPassword(password, passwordHash);
    }

    [Fact]
    public async Task HandleAsync_Should_Call_GenerateToken_With_Correct_Parameters_When_Credentials_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _tokenGenerator.Received(1).GenerateToken(user.Id, displayName);
    }

    [Fact]
    public async Task HandleAsync_Should_Not_Call_GenerateToken_When_User_Not_Found()
    {
        // Arrange
        var displayName = "nonexistentuser";
        var password = "password123";
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _tokenGenerator.DidNotReceive().GenerateToken(Arg.Any<Guid>(), Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_Should_Not_Call_GenerateToken_When_Password_Invalid()
    {
        // Arrange
        var displayName = "testuser";
        var password = "wrongpassword";
        var passwordHash = "hashed_password";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(false);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _tokenGenerator.DidNotReceive().GenerateToken(Arg.Any<Guid>(), Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_Should_Pass_CancellationToken_To_Repository()
    {
        // Arrange
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";
        var cancellationToken = new CancellationToken();

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, cancellationToken)
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        await _handler.HandleAsync(command, cancellationToken);

        // Assert
        await _userRepository.Received(1).GetByDisplayNameAsync(displayName, cancellationToken);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_AuthResponse_With_User_Id_From_Repository()
    {
        // Arrange
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Value.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_AuthResponse_With_Correct_DisplayName()
    {
        // Arrange
        var displayName = "testuser123";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Value.DisplayName.Should().Be(displayName);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_AuthResponse_With_Generated_Token()
    {
        // Arrange
        var displayName = "testuser";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "generated_jwt_token_12345";

        var user = User.Create(displayName, passwordHash);
        var command = new LoginUserCommand(displayName, password);

        _userRepository.GetByDisplayNameAsync(displayName, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(password, passwordHash)
            .Returns(true);

        _tokenGenerator.GenerateToken(user.Id, displayName)
            .Returns(token);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Value.Token.Should().Be(token);
    }
}
