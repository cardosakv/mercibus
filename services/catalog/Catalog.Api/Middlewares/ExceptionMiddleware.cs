using Catalog.Application.Common;
using Catalog.Application.DTOs;

namespace Catalog.Api.Middlewares;

/// <summary>
/// Middleware to handle exceptions globally.
/// </summary>
public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, Messages.ExceptionOccured);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new StandardResponse
            {
                Message = Messages.UnexpectedError
            });
        }
    }
}