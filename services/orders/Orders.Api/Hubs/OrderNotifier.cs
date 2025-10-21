using Microsoft.AspNetCore.SignalR;
using Orders.Application.Interfaces.Messaging;

namespace Orders.Api.Hubs;

public class OrderNotifier(IHubContext<OrderHub> orderHub) : IOrderNotifier
{
    public async Task NotifyOrderStatusAsync(long orderId, string userId, string status, CancellationToken cancellationToken = default)
    {
        await orderHub.Clients.Group(userId).SendAsync("order.status", status, cancellationToken);
    }
}