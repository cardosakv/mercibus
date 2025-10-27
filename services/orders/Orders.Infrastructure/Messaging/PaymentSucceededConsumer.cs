using MassTransit;
using Messaging.Events;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class PaymentSucceededConsumer(IOrderRepository orderRepository, AppDbContext dbContext, IOrderNotifier orderNotifier) : IConsumer<PaymentSucceeded>
{
    public async Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
        var message = context.Message;

        var order = await orderRepository.GetByIdAsync(message.OrderId);
        if (order is not null)
        {
            order.Status = OrderStatus.Confirmed;
            await orderRepository.UpdateAsync(order);
            await dbContext.SaveChangesAsync();

            await orderNotifier.NotifyOrderStatusAsync(order.Id, order.UserId, nameof(OrderStatus.Confirmed));
        }
    }
}