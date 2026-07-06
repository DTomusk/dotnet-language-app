using Api.Auth.Validators;
using Api.Submissions.Validators;
using FluentValidation;

namespace Api.IoC;

public static class Register
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateSubmissionRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();

        return services;
    }
}
