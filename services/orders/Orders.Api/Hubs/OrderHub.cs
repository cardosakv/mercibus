using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Orders.Api.Hubs;

/// <summary>
/// Hub for managing order-related real-time communications.
/// </summary>
public class OrderHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }
}