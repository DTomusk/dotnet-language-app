using Application.LanguagePractice.Interfaces;
using Application.Shared.Interfaces;
using Application.Submissions.Interfaces;
using Infrastructure.LanguagePractice.QueryServices;
using Infrastructure.LanguagePractice.Repositories;
using Infrastructure.LanguagePractice.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.LanguagePractice.IoC;

public static class Register
{
    public static IServiceCollection AddLanguagePracticeInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ISubmissionRepository, SubmissionRespository>();
        services.AddScoped<ISubmissionQueryService, SubmissionQueryService>();
        services.AddScoped<ILanguageLearnerRepository, LanguageLearnerRepository>();
        services.AddScoped<ILanguageLearnerQueryService, LanguageLearnerQueryService>();
        services.AddScoped<ILanguageAnalysisService, LanguageAnalysisService>();
        services.AddScoped<ILanguageAnalysisRepository, LanguageAnalysisRepository>();
        services.AddScoped<IHealthCheck, LanguageAnalysisService>();
        return services;
    }
}
