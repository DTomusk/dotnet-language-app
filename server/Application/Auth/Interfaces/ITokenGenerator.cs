namespace Application.Auth.Interfaces;

public interface ITokenGenerator
{
    string GenerateToken(Guid userId, string displayName);
}
