using Messaging.Models;

namespace Messaging.Events;

/// <summary>
/// Event representing the creation of an order.
/// </summary>
public record OrderCreated(
    long OrderId,
    string CustomerId,
    List<OrderItem> Items,
    DateTime CreatedAt
);