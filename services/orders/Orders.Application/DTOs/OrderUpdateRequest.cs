namespace Orders.Application.DTOs;

/// <summary>
/// Represents a request to update an order.
/// </summary>
/// <param name="Status">Order status.</param>
public record OrderUpdateRequest(
    string Status
);