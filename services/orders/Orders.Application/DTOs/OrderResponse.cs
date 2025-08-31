namespace Orders.Application.DTOs;

/// <summary>
/// Represents a response containing order details.
/// </summary>
public record OrderResponse(
    long Id,
    string UserId,
    DateTime OrderDate,
    List<OrderItemResponse> Items
);