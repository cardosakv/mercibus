namespace Orders.Application.DTOs;

/// <summary>
/// Represents an item in an order response.
/// </summary>
public record OrderItemResponse(
    long Id,
    long ProductId,
    string ProductName,
    decimal Price,
    int Quantity
);