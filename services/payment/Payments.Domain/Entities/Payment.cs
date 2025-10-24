using Payments.Domain.Enums;

namespace Payments.Domain.Entities;

/// <summary>
/// Represents a payment entity in the system.
/// </summary>
public class Payment
{
    /// <summary>
    /// Unique identifier for the payment.
    /// Used as a reference ID in the payment gateway.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Identifier for the associated order.
    /// </summary>
    public required long OrderId { get; set; }

    /// <summary>
    /// Identifier for the customer making the payment.
    /// </summary>
    public required string CustomerId { get; set; }

    /// <summary>
    /// Amount to be paid.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency of the payment amount.
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Current status of the payment.
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.AwaitingUserAction;
    
    /// <summary>
    /// Timestamp when the payment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Timestamp when the payment was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}