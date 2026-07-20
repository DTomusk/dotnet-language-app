namespace Domain.Shared.Results;

public sealed record Error(string Message, ErrorType Type)
{
    public static Error Validation(string message) => new(message, ErrorType.Validation);
    public static Error NotFound(string message) => new(message, ErrorType.NotFound);
    public static Error Conflict(string message) => new(message, ErrorType.Conflict);
    public static Error Unauthorized(string message) => new(message, ErrorType.Unauthorized);
    public static Error Forbidden(string message) => new(message, ErrorType.Forbidden);
    public static Error Internal(string message) => new(message, ErrorType.Internal);
}

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Internal
}
