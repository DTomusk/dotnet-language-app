using Application.Shared.Interfaces;
using Infrastructure.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Shared.IoC;

public static class Register
{
    public static IServiceCollection AddSharedInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Db context
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DATABASE_URL"));
        });

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<EventDispatcher, EventDispatcher>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();

        // Automatically starts the outbox processor service when the application starts
        // TODO: in a microservice architecture, each deployable would have its own published and outbox processor (rather than one central one)
        services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
