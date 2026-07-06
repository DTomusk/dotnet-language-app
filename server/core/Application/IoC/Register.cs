using Core.Application.Commands;
using Core.Application.DTOs;
using Core.Application.Handlers;
using Core.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.IoC;

public static class Register
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateSubmissionCommand, Guid>, CreateSubmissionCommandHandler>();
        services.AddScoped<ICommandHandler<RegisterUserCommand, AuthResponse>, RegisterUserCommandHandler>();

        return services;
    }
}
