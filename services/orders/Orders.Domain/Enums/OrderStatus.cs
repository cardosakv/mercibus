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
    /// Order stock has been successfully reserved.
    /// </summary>
    StockReserved,

    /// <summary>
    /// Order stock reservation failed.
    /// </summary>
    StockFailed,

    /// <summary>
    /// Payment is not successful.
    /// </summary>
    PaymentFailed,

    /// <summary>
    /// Order has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Order has been processed successfully.
    /// </summary>
    Confirmed
}