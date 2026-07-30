using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Interfaces;
using Domain.LanguagePractice.ValueObjects;
using Infrastructure.LanguagePractice.Configuration;
using Infrastructure.LanguagePractice.QueryServices;
using Infrastructure.LanguagePractice.Repositories;
using Infrastructure.LanguagePractice.Services;
using Infrastructure.LanguagePractice.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.LanguagePractice.IoC;

public static class Register
{
    public static IServiceCollection AddLanguagePracticeInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LanguageAnalysisApiOptions>(configuration.GetSection(LanguageAnalysisApiOptions.SectionName));
        services.AddHttpClient<ILanguageAnalysisService, LanguageAnalysisService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<LanguageAnalysisApiOptions>>().Value;

            client.BaseAddress = new Uri(options.BaseUrl);

            client.Timeout = options.Timeout;
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<ISubmissionRepository, SubmissionRespository>();
        services.AddScoped<ISubmissionQueryService, SubmissionQueryService>();
        services.AddScoped<ILanguageLearnerRepository, LanguageLearnerRepository>();
        services.AddScoped<ILanguageLearnerQueryService, LanguageLearnerQueryService>();
        services.AddScoped<ILanguageAnalysisRepository, LanguageAnalysisRepository>();

        // Validation strategies
        services.AddScoped<ILanguageValidationStrategy, ItalianValidationStrategy>();
        services.AddScoped<ILanguageValidationStrategy, GermanValidationStrategy>();

        services.AddScoped<IDictionary<LanguageCode, ILanguageValidationStrategy>>(serviceProvider =>
        {
            var strategies = serviceProvider.GetServices<ILanguageValidationStrategy>().ToList();
            return new Dictionary<LanguageCode, ILanguageValidationStrategy>
            {
                { LanguageCode.Italian, strategies.OfType<ItalianValidationStrategy>().First() },
                { LanguageCode.German, strategies.OfType<GermanValidationStrategy>().First() }
            };
        });

        // Cast ILanguageAnalysisService to IHealthCheck for health check registration as the concrete type implements both
        services.AddScoped(sp => (IHealthCheck)sp.GetRequiredService<ILanguageAnalysisService>());
        return services;
    }
}
