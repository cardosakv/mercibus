namespace Orders.Application.DTOs;

/// <summary>
/// Represents a request to create a new order.
/// </summary>
public record OrderRequest(
    string UserId,
    List<OrderItemRequest> Items
);