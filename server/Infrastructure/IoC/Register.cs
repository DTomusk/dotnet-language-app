using Infrastructure.Auth.IoC;
using Infrastructure.LanguagePractice.IoC;
using Infrastructure.Shared.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC;

public static class Register
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLanguagePracticeInfrastructureServices();
        services.AddAuthInfrastructureServices(configuration);
        services.AddSharedInfrastructureServices(configuration);

        return services;
    }
}
