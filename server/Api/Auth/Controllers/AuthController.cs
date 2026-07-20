using Api.Auth.DTOs;
using Api.Shared.Extensions;
using Application.Auth.Commands;
using Application.Auth.DTOs;
using Application.Shared.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICommandHandler<LoginUserCommand, AuthResponse> _loginHandler;
    private readonly ICommandHandler<RegisterUserCommand, AuthResponse> _registerHandler;

    public AuthController(
        ICommandHandler<LoginUserCommand, AuthResponse> loginHandler,
        ICommandHandler<RegisterUserCommand, AuthResponse> registerHandler)
    {
        _loginHandler = loginHandler;
        _registerHandler = registerHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new RegisterUserCommand(request.DisplayName, request.Password);
        var authResult = await _registerHandler.HandleAsync(command, cancellationToken);

        return authResult.ToActionResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.DisplayName, request.Password);
        var authResult = await _loginHandler.HandleAsync(command, cancellationToken);

        return authResult.ToActionResult();
    }
}
