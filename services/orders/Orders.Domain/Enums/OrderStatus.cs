namespace Orders.Domain.Enums;

/// <summary>
/// Represents the status of an order.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Created but not yet processed.
    /// </summary>
    Pending,    
    
    /// <summary>
    /// Done payment and currently being processed.
    /// </summary>
    Processing,
    
    /// <summary>
    /// Ready for shipment.
    /// </summary>
    Shipped,
    
    /// <summary>
    /// Delivered to the customer.
    /// </summary>
    Delivered,
    
    /// <summary>
    /// Order has been cancelled.
    /// </summary>
    Cancelled
}