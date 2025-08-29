using Auth.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;
using Testcontainers.PostgreSql;

namespace Auth.IntegrationTests;

/// <summary>
/// Factory for creating a test web application with a PostgresSQL database.
/// </summary>
public class TestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public TestWebAppFactory()
    {
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        _azuriteContainer.StartAsync().GetAwaiter().GetResult();
    }

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private readonly AzuriteContainer _azuriteContainer = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
        .WithCommand("--skipApiVersionCheck")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var connectionString = _dbContainer.GetConnectionString();
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        });

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    {
                        "ConnectionStrings:BlobStorageConnection", _azuriteContainer.GetConnectionString()
                    },
                    {
                        "BlobStorage:AccountName", "devstoreaccount1"
                    },
                    {
                        "BlobStorage:AccountKey", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
                    },
                    {
                        "Jwt:PublicKeyPath", "test_pub_key.pem"
                    },
                    {
                        "Jwt:PrivateKeyPath", "test_priv_key.pem"
                    }
                }!);
        });
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _azuriteContainer.StopAsync();
    }
}