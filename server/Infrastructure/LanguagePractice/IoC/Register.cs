using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Interfaces;
using Infrastructure.LanguagePractice.Configuration;
using Infrastructure.LanguagePractice.QueryServices;
using Infrastructure.LanguagePractice.Repositories;
using Infrastructure.LanguagePractice.Services;
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

        // Cast ILanguageAnalysisService to IHealthCheck for health check registration as the concrete type implements both
        services.AddScoped<IHealthCheck>(sp => (IHealthCheck)sp.GetRequiredService<ILanguageAnalysisService>());
        return services;
    }
}
