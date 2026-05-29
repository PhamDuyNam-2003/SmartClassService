namespace auth_service.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services
    )
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(
        this WebApplication app
    )
    {
        app.UseSwagger();

        app.UseSwaggerUI();

        return app;
    }
}