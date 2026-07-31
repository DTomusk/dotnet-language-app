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

        return services;
    }
}
