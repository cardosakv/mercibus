using MassTransit;
using Messaging.Events;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class StockReservedConsumer(IOrderService orderService, IOrderNotifier orderNotifier, IEventPublisher eventPublisher) : IConsumer<StockReserved>
{
    public async Task Consume(ConsumeContext<StockReserved> context)
    {
        var message = context.Message;

        var result = await orderService.GetByIdAsync(message.OrderId, context.CancellationToken);
        if (result.Data is not OrderResponse order)
        {
            return;
        }

        await orderService.UpdateAsync(order.Id, new OrderUpdateRequest(nameof(OrderStatus.PendingPayment)), context.CancellationToken);
        await eventPublisher.PublishAsync(
            new OrderPendingPayment(
                OrderId: order.Id,
                CustomerId: order.UserId,
                TotalAmount: order.Items.Sum(i => i.Price * i.Quantity),
                Currency: order.Currency
            ),
            context.CancellationToken);

        await orderNotifier.NotifyOrderStatusAsync(message.OrderId, message.CustomerId, nameof(OrderStatus.PendingPayment), context.CancellationToken);
    }
}