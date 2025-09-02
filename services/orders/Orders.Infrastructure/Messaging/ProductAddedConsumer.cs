using MassTransit;
using Messaging.Events;
using Orders.Application.Interfaces.Services;

namespace Orders.Infrastructure.Messaging;

public class ProductAddedConsumer(IProductReadService productReadService) : IConsumer<ProductAdded>
{
    public async Task Consume(ConsumeContext<ProductAdded> context)
    {
        var message = context.Message;
        await productReadService.AddAsync(message.ProductId);
    }
}