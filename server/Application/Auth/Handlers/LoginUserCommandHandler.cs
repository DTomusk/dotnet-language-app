using Application.Auth.Commands;
using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Shared.Interfaces;
using Domain.Shared.Results;

namespace Application.Auth.Handlers;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator) : ICommandHandler<LoginUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<Result<AuthResponse>> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByDisplayNameAsync(command.DisplayName, cancellationToken);
        if (user == null || !_passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure(new Error("Invalid display name or password.", ErrorType.Validation));

        var token = _tokenGenerator.GenerateToken(user.Id, user.DisplayName);
        return Result<AuthResponse>.Success(new AuthResponse(user.Id, user.DisplayName, token));
    }
}
