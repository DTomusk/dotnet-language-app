using Application.Auth.Commands;
using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Shared.Interfaces;
using Domain.Auth.Entities;

namespace Application.Auth.Handlers;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthResponse> HandleAsync(
        RegisterUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByDisplayNameAsync(command.DisplayName, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with the same display name already exists.");
        }

        var passwordHash = _passwordHasher.HashPassword(command.Password);
        var user = User.Create(command.DisplayName, passwordHash);

        await _userRepository.CreateAsync(user, cancellationToken);

        var token = _tokenGenerator.GenerateToken(user.Id, user.DisplayName);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthResponse(user.Id, user.DisplayName, token);
    }
}
