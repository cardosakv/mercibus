using Microsoft.AspNetCore.SignalR;
using Orders.Application.Common;
using Orders.Application.Interfaces.Messaging;

namespace Orders.Api.Hubs;

public class OrderNotifier(IHubContext<OrderHub> orderHub) : IOrderNotifier
{
    public async Task NotifyOrderStatusAsync(long orderId, string userId, string status, CancellationToken cancellationToken = default)
    {
        await orderHub.Clients.Group(userId).SendAsync(Constants.Hub.OrderStatusMethod, status, cancellationToken);
    }
}