using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.Azurite;
using Testcontainers.PostgreSql;

namespace Auth.IntegrationTests;

/// <summary>
/// Factory for creating a test web application with a PostgresSQL database.
/// </summary>
public class TestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private readonly AzuriteContainer _azuriteContainer = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
        .WithCommand("--skipApiVersionCheck")
        .Build();
    
    public TestWebAppFactory()
    {
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        _azuriteContainer.StartAsync().GetAwaiter().GetResult();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    {
                        "ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString()
                    },
                    {
                        "ConnectionStrings:BlobStorageConnection", _azuriteContainer.GetConnectionString()
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
    
    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _azuriteContainer.StopAsync();
    }
}