using MassTransit;
using Orders.Infrastructure.Messaging;

namespace Orders.Api.Extensions;

/// <summary>
/// Extension methods for messaging configuration.
/// </summary>
public static class MessagingExtension
{
    public static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductAddedConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMq:Host"],
                    h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]!);
                        h.Password(configuration["RabbitMq:Password"]!);
                    });
                
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}