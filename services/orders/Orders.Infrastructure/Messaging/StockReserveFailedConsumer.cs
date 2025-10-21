using MassTransit;
using Messaging.Events;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Services;
using Orders.Domain.Enums;

namespace Orders.Infrastructure.Messaging;

public class StockReserveFailedConsumer(IOrderService orderService, IOrderNotifier orderNotifier) : IConsumer<StockReserveFailed>
{
    public async Task Consume(ConsumeContext<StockReserveFailed> context)
    {
        var message = context.Message;

        await orderService.UpdateAsync(message.OrderId, new OrderUpdateRequest(nameof(OrderStatus.StockFailed)), context.CancellationToken);
        await orderNotifier.NotifyOrderStatusAsync(message.OrderId, message.CustomerId, nameof(OrderStatus.StockFailed), context.CancellationToken);
    }
}