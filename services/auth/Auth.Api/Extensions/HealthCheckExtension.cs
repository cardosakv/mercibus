using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Auth.Api.Extensions;

/// <summary>
/// Extension methods for adding health checks.
/// </summary>
public static class HealthCheckExtension
{
    public static void AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection") ?? string.Empty);
    }

    public static void MapCustomHealthChecks(this IEndpointRouteBuilder endpoints, string healthCheckEndpoint = "/health")
    {
        endpoints.MapHealthChecks(
            healthCheckEndpoint,
            new HealthCheckOptions
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
                                exception = entry.Value.Exception?.Message,
                            })
                        });

                    await context.Response.WriteAsync(result);
                }
            });
    }
}