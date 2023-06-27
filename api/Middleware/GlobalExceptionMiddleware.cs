using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using api.TransferObjects;

namespace api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext http)
    {
        try
        {
            await _next.Invoke(http);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(http, exception, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext http, Exception exception, ILogger<GlobalExceptionMiddleware> logger)
    {
        http.Response.ContentType = "application/json";

        if (exception is ValidationException ||
            exception is ArgumentException ||
            exception is ArgumentNullException ||
            exception is ArgumentOutOfRangeException ||
            exception is InvalidCredentialException)
        {
            http.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else if (exception is KeyNotFoundException)
        {
            http.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else if (exception is AuthenticationException)
        {
            http.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else if (exception is UnauthorizedAccessException)
        {
            http.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else if (exception is NotSupportedException ||
                 exception is NotImplementedException)
        {
            http.Response.StatusCode = StatusCodes.Status501NotImplemented;
        }
        else
        {
            http.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        logger.LogError(exception, "{ExceptionMessage}", exception.Message);
        return http.Response.WriteAsJsonAsync(new ResponseDto(exception.Message));
    }
}