namespace Auth.Api.Middlewares;

/// <summary>
/// Logging middleware for request and response.
/// </summary>
public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation(
            "Request received: {RequestMethod} {RequestPath}",
            context.Request.Method, context.Request.Path);

        await next(context);

        logger.LogInformation(
            "Request finished: {RequestMethod} {RequestPath} {ResponseCode}",
            context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}

/// <summary>
/// Extension methods for the LoggingMiddleware.
/// </summary>
public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}