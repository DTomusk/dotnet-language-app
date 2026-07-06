namespace Core.Application.DTOs;

public record AuthResponse(Guid UserId, string DisplayName, string Token);