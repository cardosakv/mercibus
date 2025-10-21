namespace Orders.Application.Interfaces.Messaging;

/// <summary>
/// Interface for notifying clients about order status updates.
/// </summary>
public interface IOrderNotifier
{
    /// <summary>
    /// Notify clients about order status update.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <param name="userId">The identifier of the user associated with the order.</param>
    /// <param name="status">The updated status of the order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task NotifyOrderStatusAsync(long orderId, string userId, string status, CancellationToken cancellationToken = default);
}