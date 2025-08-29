using Catalog.Application.Interfaces;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static Npgsql.NpgsqlConnection;

#pragma warning disable CS0618

namespace Catalog.Api.Extensions;

/// <summary>
///     Extension methods for databases.
/// </summary>
public static class DatabaseExtension
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        GlobalTypeMapper.EnableDynamicJson();
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }

    public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var connectionString = configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(connectionString ?? string.Empty);
        });
    }
}