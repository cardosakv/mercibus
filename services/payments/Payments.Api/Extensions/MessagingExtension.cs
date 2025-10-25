using MassTransit;
using Messaging.Events;
using Payments.Infrastructure.Messaging;

namespace Payments.Api.Extensions;

/// <summary>
/// Messaging extensions for service collection.
/// </summary>
public static class MessagingExtension
{
    public static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPendingPaymentConsumer>();

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