using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Orders.Application.Common;
using Orders.Application.Interfaces.Repositories;
using Orders.Infrastructure;

namespace Orders.Api.Extensions;

/// <summary>
/// Database extensions for service collection.
/// </summary>
public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(defaultConnectionString));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        var mongoConnectionString = configuration.GetConnectionString("MongoConnection");
        services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoConnectionString));
        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(Constants.Mongo.DatabaseName);
        });
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }
}