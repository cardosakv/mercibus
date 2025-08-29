using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Catalog.Api.Extensions;

/// <summary>
///     Extension methods for adding health checks.
/// </summary>
public static class HealthCheckExtension
{
    public static void AddCustomHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        if (builder.Environment.IsStaging())
            return;

        services.AddHealthChecks()
            .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
            .AddRedis(builder.Configuration.GetConnectionString("RedisConnection") ?? string.Empty);
    }

    public static void MapCustomHealthChecks(this IEndpointRouteBuilder endpoints, WebApplicationBuilder builder)
    {
        if (builder.Environment.IsStaging())
        {
            return;
        }

        endpoints.MapHealthChecks(
            pattern: "/health",
            options: new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(entry => new
                            {
                                component = entry.Key,
                                status = entry.Value.Status.ToString(),
                                exception = entry.Value.Exception?.Message
                            })
                        });

                    await context.Response.WriteAsync(result);
                }
            });
    }
}