namespace Auth.Api.Middlewares;

/// <summary>
/// Logging middleware for request and response.
/// </summary>
public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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