using Messaging.Models;

namespace Messaging.Events;

/// <summary>
/// Event representing a failed order.
/// </summary>
public record OrderFailed(
    long OrderId,
    string CustomerId,
    List<OrderItem> Items,
    DateTime FailedAt
);