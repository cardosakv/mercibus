using Mercibus.Common.Responses;
using Mercibus.Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Mercibus.Common.Middlewares;

/// <summary>
/// Middleware to handle 401 Unauthorized responses globally.
/// </summary>
public class CustomAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Type = ErrorType.AuthenticationError,
                    Code = ErrorCode.Unauthorized
                }
            };

            var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(json);
        }

        if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Type = ErrorType.PermissionError,
                    Code = ErrorCode.AccessDenied
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
///  Extension methods for the CustomAuthMiddleware.
/// </summary>
public static class CustomAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomAuthMiddleware>();
    }
}