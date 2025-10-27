using MassTransit;
using Payments.Application.Interfaces.Messaging;

namespace Payments.Infrastructure.Messaging;

public class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : class
    {
        await publishEndpoint.Publish(eventMessage, cancellationToken);
    }
}