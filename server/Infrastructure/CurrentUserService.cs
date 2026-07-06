using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId 
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : (Guid?)null;
        }
    }

    public bool IsAuthenticated => throw new NotImplementedException();
}
