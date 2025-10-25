namespace Orders.Application.DTOs;

/// <summary>
/// Represents a request to add an item to an order.
/// </summary>
public record OrderItemRequest(
    long ProductId,
    string ProductName,
    int Quantity,
    decimal Price
);