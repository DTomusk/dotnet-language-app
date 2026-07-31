using Application.Shared.Interfaces;
using Infrastructure.Shared.Events;

namespace Worker.IoC;

public static class Register
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<EventDispatcher, EventDispatcher>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.Configure<OutboxProcessorOptions>(configuration.GetSection(OutboxProcessorOptions.SectionName));
        return services;
    }
}
