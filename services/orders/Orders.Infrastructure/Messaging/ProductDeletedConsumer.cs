using MassTransit;
using Messaging.Events;
using Orders.Application.Interfaces.Services;

namespace Orders.Infrastructure.Messaging;

public class ProductDeletedConsumer(IProductReadService productReadService) : IConsumer<ProductDeleted>
{
    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        var message = context.Message;
        await productReadService.DeleteAsync(message.ProductId);
    }
}