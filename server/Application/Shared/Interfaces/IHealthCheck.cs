namespace Application.Shared.Interfaces;

public interface IHealthCheck
{
    Task<HealthCheckResult> IsHealthy();
}

public record HealthCheckResult(string Name, bool IsHealthy, string? Message = null);