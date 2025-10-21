using Catalog.Application.Common;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Infrastructure.Repositories;
using MassTransit;
using Messaging.Events;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Messaging;

public class OrderCreatedConsumer(IProductRepository productRepository, AppDbContext dbContext, ILogger<ProductRepository> logger) : IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var order = context.Message;
        var unavailableProducts = new List<long>();

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        try
        {
            foreach (var item in order.Items)
            {
                var isSuccess = await productRepository.ReduceProductStockAsync(item.ProductId, item.Quantity, CancellationToken.None);
                if (!isSuccess)
                {
                    unavailableProducts.Add(item.ProductId);
                }
            }
            
            if (unavailableProducts.Count > 0)
            {
                await transaction.RollbackAsync();
                await context.Publish(new StockReserveFailed(order.OrderId, order.CustomerId, unavailableProducts) );
            }
            else
            {
                await transaction.CommitAsync();
                await context.Publish(new StockReserved(order.OrderId, order.CustomerId));
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            await context.Publish(new StockReserveFailed(order.OrderId, order.CustomerId, order.Items.Select(o => o.ProductId).ToList()) );
            logger.LogError(ex, Constants.Messages.ExceptionOccurred);
        }
    }
}