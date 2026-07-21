using Api.RateLimiting;
using Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Shared.Controllers;

[Authorize]
[ApiController]
public class AuthenticatedControllerBase : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;

    protected AuthenticatedControllerBase(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected Guid CurrentUserId => _currentUserService.UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");
}
