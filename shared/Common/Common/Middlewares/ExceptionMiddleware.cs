using Common.Constants;
using Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Middlewares;

/// <summary>
/// Middleware to handle exceptions globally.
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurred while processing the request.");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Type = ErrorType.ApiError,
                    Code = ErrorCode.Internal
                }
            };

            var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(json);
        }
    }
}

/// <summary>
/// Extension methods for the ExceptionMiddleware.
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}