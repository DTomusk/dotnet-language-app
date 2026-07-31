using Application.Shared.Interfaces;
using Worker.Services;

namespace Worker.IoC;

public static class Register
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<EventDispatcher, EventDispatcher>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.Configure<OutboxProcessorOptions>(configuration.GetSection(OutboxProcessorOptions.SectionName));

        services.AddHostedService<OutboxProcessorService>();
        return services;
    }
}
