using Api.DTOs;
using Core.Application.Commands;
using Core.Application.DTOs;
using Core.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ICommandHandler<RegisterUserCommand, AuthResponse> _registerHandler;
    private readonly IValidator<RegisterUserRequest> _registerValidator;

    public AuthController(
        ICommandHandler<RegisterUserCommand, AuthResponse> registerHandler,
        IValidator<RegisterUserRequest> registerValidator)
    {
        _registerHandler = registerHandler;
        _registerValidator = registerValidator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _registerValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var command = new RegisterUserCommand(request.DisplayName, request.Password);
            var authResponse = await _registerHandler.HandleAsync(command, cancellationToken);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
