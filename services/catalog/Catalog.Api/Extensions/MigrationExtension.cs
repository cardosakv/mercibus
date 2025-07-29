using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Extensions;

/// <summary>
/// Extension methods for applying migrations in the application.
/// </summary>
public static class MigrationExtension
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }
}