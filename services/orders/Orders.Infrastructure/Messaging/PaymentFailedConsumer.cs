using MassTransit;
using Messaging.Events;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class PaymentFailedConsumer(IOrderRepository orderRepository, AppDbContext dbContext, IOrderNotifier orderNotifier) : IConsumer<PaymentFailed>
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
        }
    }
}