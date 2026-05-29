using auth_service.Middleware;

namespace auth_service.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication UseAppMiddlewares(
        this WebApplication app
    )
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}