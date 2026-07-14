using Application.Auth.Interfaces;
using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Interfaces;
using Infrastructure.Auth;
using Infrastructure.LanguagePractice;
using Infrastructure.Shared;
using Infrastructure.Shared.Events;
using Infrastructure.Submissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC;

public static class Register
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Db context
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DATABASE_URL"));
        });

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenGenerator, JwtGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRespository>();
        services.AddScoped<ILanguageLearnerRepository, LanguageLearnerRepository>();
        services.AddScoped<ISubmissionQueryService, SubmissionQueryService>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<EventDispatcher, EventDispatcher>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<ILanguageLearnerQueryService, LanguageLearnerQueryService>();
        services.AddScoped<ILanguageAnalysisService, LanguageAnalysisService>();
        services.AddScoped<ILanguageAnalysisRepository, LanguageAnalysisRepository>();

        // Automatically starts the outbox processor service when the application starts
        services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
