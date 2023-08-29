using System.Net;
using System.Text.Json;

namespace PhysicalPersonDirectory.Api.Middlewares;

public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

    public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var status = HttpStatusCode.InternalServerError;
        var type = exception.GetType();
        if (type == typeof(ArgumentNullException) || type == typeof(ArgumentException))
        {
            status = HttpStatusCode.BadRequest;
        }

        var exceptionResult = JsonSerializer.Serialize(new
        {
            error = exception.Message,
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(exceptionResult);
    }
}