using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Api.Shared.RateLimiting;

public static class RateLimitingConfiguration
{
    public const string AuthPolicy = "AuthPolicy";
    public const string AuthenticatedPolicy = "AuthenticatedPolicy";

    public static RateLimiterOptions ConfigureRateLimiting(
        this RateLimiterOptions options,
        IConfiguration configuration)
    {
        var rateLimitOptions = configuration.GetSection("RateLimitingOptions");
        var authLimit = rateLimitOptions.GetValue<int>("AuthEndpointLimit", 10);
        var authenticatedLimit = rateLimitOptions.GetValue<int>("AuthenticatedEndpointLimit", 100);
        var windowMinutes = rateLimitOptions.GetValue<int>("WindowMinutes", 1);

        // Auth policy: stricter limit per IP address
        options.AddPolicy(AuthPolicy, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = authLimit,
                    Window = TimeSpan.FromMinutes(windowMinutes),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0 // No queuing for auth endpoints
                }));

        // Authenticated policy: more lenient limit per user ID
        options.AddPolicy(AuthenticatedPolicy, httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                factory: partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = authenticatedLimit,
                    Window = TimeSpan.FromMinutes(windowMinutes),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                }));

        return options;
    }
}
