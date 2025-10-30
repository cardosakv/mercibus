using MassTransit;
using Messaging.Events;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Messaging;

public class OrderFailedConsumer(AppDbContext dbContext) : IConsumer<OrderFailed>
{
    public async Task Consume(ConsumeContext<OrderFailed> context)
    {
        var message = context.Message;

        foreach (var item in message.Items)
        {
            await dbContext.Products
                .Where(p => p.Id == item.ProductId)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(
                        x => x.StockQuantity,
                        x => x.StockQuantity + item.Quantity
                    )
                );
        }
    }
}