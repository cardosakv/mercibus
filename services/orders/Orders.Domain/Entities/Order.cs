using Orders.Domain.Enums;

namespace Orders.Domain.Entities;

/// <summary>
/// Represents an order placed by a user.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for the order.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identifier of the user who placed the order.
    /// </summary>
    public required string UserId { get; set; }
    
    /// <summary>
    /// Current status of the order.
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    /// <summary>
    /// Date and time when the order was placed.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Items included in the order.
    /// </summary>
    public ICollection<OrderItem> Items { get; set; } = [];
}