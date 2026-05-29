using auth_service.Services.Implement;
using auth_service.Services.Interfaces;

namespace auth_service.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services
    )
    {
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IAuthService, AuthService>();
     

        return services;
    }
}  