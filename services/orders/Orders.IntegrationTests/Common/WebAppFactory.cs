using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Infrastructure;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Orders.IntegrationTests.Common;

/// <summary>
/// A custom WebApplicationFactory for the Orders Service integration tests.
/// Uses PostgreSQL (for EF Core) and RabbitMQ (for event messaging).
/// </summary>
public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:latest")
        .WithPortBinding(27017, assignRandomHostPort: true)
        .Build();

    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("orders_test_db")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:4-management")
        .WithUsername("guest")
        .WithPassword("guest")
        .WithPortBinding(5672, assignRandomHostPort: true)
        .Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _mongoContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _mongoContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
    }

    /// <summary>
    /// Configures the Orders Service test host to use test containers.
    /// </summary>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextDescriptor is not null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_postgresContainer.GetConnectionString()));
            services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
        });

        // Inject RabbitMQ settings for event publishing
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    {
                        "ConnectionStrings:MongoConnection", _mongoContainer.GetConnectionString()
                    },
                    {
                        "RabbitMq:Host", _rabbitMqContainer.GetConnectionString()
                    },
                    {
                        "RabbitMq:Username", "guest"
                    },
                    {
                        "RabbitMq:Password", "guest"
                    }
                });
        });
    }

    public AppDbContext CreateDbContext()
    {
        return Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public string GetMongoConnectionString()
    {
        return _mongoContainer.GetConnectionString();
    }
}