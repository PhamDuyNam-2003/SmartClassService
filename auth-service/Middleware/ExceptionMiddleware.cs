using auth_service.DTOs.Responses;
using auth_service.Exceptions;
using auth_service.Exceptions.Common;
using System.Net;
using System.Text.Json;

namespace auth_service.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType =
                "application/json";

            var statusCode =
                ex switch
                {
                    NotFoundException =>
                        HttpStatusCode.NotFound,

                    BadRequestException =>
                        HttpStatusCode.BadRequest,

                    UnauthorizedException =>
                        HttpStatusCode.Unauthorized,

                    _ =>
                        HttpStatusCode.InternalServerError
                };

            context.Response.StatusCode =
                (int)statusCode;

            var response =
                ApiResponse<string>.FailResponse(
                    ex.Message,
                    (int)statusCode,
                    new List<string>
                    {
                        ex.Message
                    }
                );

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}