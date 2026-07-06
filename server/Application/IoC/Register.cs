using Application.Commands;
using Application.DTOs; 
using Application.Handlers;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC;

public static class Register
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateSubmissionCommand, Guid>, CreateSubmissionCommandHandler>();
        services.AddScoped<ICommandHandler<RegisterUserCommand, AuthResponse>, RegisterUserCommandHandler>();

        return services;
    }
}
