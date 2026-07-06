namespace Application.Auth.DTOs;

public record AuthResponse(Guid UserId, string DisplayName, string Token);