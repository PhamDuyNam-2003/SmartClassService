using System.Text.Json.Serialization;

namespace auth_service.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDatabase(configuration);

        services.AddJwtAuthentication(configuration);

        services.AddAuthorization();

        services.AddApplicationServices();

        services.AddValidation();

        services.AddControllers();
        services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions
                        .Converters
                        .Add(
                            new JsonStringEnumConverter()
                        );
                });
        services.AddSwaggerDocumentation();

        return services;
    }
}