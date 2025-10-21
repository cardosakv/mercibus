namespace Messaging.Models;

/// <summary>
/// Represents an item within an order.
/// </summary>
public record OrderItem(
    long ProductId,
    int Quantity
);