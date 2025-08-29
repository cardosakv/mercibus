using Catalog.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Catalog.IntegrationTests.Common;

/// <summary>
/// A custom WebApplicationFactory for integration tests with database and blob storage.
/// </summary>
public class BlobWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private readonly AzuriteContainer _azuriteContainer = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
        .WithCommand("--skipApiVersionCheck")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis/redis-stack")
        .WithPortBinding(port: 6379, assignRandomHostPort: true)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Staging");
        
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    {
                        "ConnectionStrings:DefaultConnection", _postgresContainer.GetConnectionString()
                    },
                    {
                        "ConnectionStrings:RedisConnection", _redisContainer.GetConnectionString()
                    },
                    {
                        "ConnectionStrings:BlobStorageConnection", _azuriteContainer.GetConnectionString()
                    }
                }!);
        });

        builder.ConfigureServices(services =>
        {
            // Remove existing AppDbContext registration since this is injected in IAppDbContext
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_postgresContainer.GetConnectionString()));
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(authenticationScheme: "Test", _ => { });
        });
    }

    public AppDbContext CreateDbContext()
    {
        return Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _azuriteContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _azuriteContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}