using MassTransit;
using Messaging.Events;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class StockReservedConsumer(IOrderService orderService, IOrderNotifier orderNotifier) : IConsumer<StockReserved>
{
    public async Task Consume(ConsumeContext<StockReserved> context)
    {
        var message = context.Message;

        await orderService.UpdateAsync(message.OrderId, new OrderUpdateRequest(nameof(OrderStatus.PendingPayment)), context.CancellationToken);
        await orderNotifier.NotifyOrderStatusAsync(message.OrderId, message.CustomerId, nameof(OrderStatus.PendingPayment), context.CancellationToken);
    }
}