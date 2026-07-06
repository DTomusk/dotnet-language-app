using Core.Application.Commands;
using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Domain.Entities;

namespace Core.Application.Handlers;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator) : ICommandHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<AuthResponse> HandleAsync(
        RegisterUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByDisplayNameAsync(command.DisplayName, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with the same display name already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            DisplayName = command.DisplayName,
            PasswordHash = _passwordHasher.HashPassword(command.Password)
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        var token = _tokenGenerator.GenerateToken(user.Id, user.DisplayName);

        return new AuthResponse(user.Id, user.DisplayName, token);
    }
}
