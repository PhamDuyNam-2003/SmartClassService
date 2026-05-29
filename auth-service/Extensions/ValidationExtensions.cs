using FluentValidation;
using FluentValidation.AspNetCore;

namespace auth_service.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(
        this IServiceCollection services
    )
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}