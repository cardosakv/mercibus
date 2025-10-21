namespace Orders.Domain.Entities;

/// <summary>
/// Represents an item within an order.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identifier of the order to which this item belongs.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// The order to which this item belongs.
    /// </summary>
    public Order Order { get; set; } = null!;

    /// <summary>
    /// Identifier of the product.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// Price of a single unit of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the product ordered.
    /// </summary>
    public int Quantity { get; set; }
}