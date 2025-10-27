using Catalog.Infrastructure.Messaging;
using MassTransit;

namespace Catalog.Api.Extensions;

public static class MessagingExtension
{
    public static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();
            x.AddConsumer<OrderFailedConsumer>();

            x.UsingRabbitMq((context, config) =>
            {
                config.Host(
                    host: configuration["RabbitMq:Host"],
                    h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]!);
                        h.Password(configuration["RabbitMq:Password"]!);
                    });

                config.ConfigureEndpoints(context);
            });
        });
    }
}