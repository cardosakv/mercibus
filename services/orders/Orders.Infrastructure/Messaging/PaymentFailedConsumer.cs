using MassTransit;
using Messaging.Events;
using Messaging.Models;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class PaymentFailedConsumer(IOrderRepository orderRepository, AppDbContext dbContext, IOrderNotifier orderNotifier, IEventPublisher eventPublisher) : IConsumer<PaymentFailed>
{
    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var message = context.Message;

        var order = await orderRepository.GetByIdAsync(message.OrderId);
        if (order is not null)
        {
            order.Status = OrderStatus.PaymentFailed;
            await orderRepository.UpdateAsync(order);
            await dbContext.SaveChangesAsync();

            await orderNotifier.NotifyOrderStatusAsync(order.Id, order.UserId, nameof(OrderStatus.PaymentFailed));
            await eventPublisher.PublishAsync(
                new OrderFailed(
                    order.Id,
                    message.CustomerId,
                    order.Items.Select(x => new OrderItem(x.ProductId, x.Quantity)).ToList(),
                    DateTime.UtcNow
                ));
        }
    }
}