namespace Messaging.Events;

/// <summary>
/// Event representing a failed payment.
/// </summary>
public record PaymentFailed(
    long PaymentId,
    long OrderId,
    string CustomerId,
    decimal TotalAmount,
    string Currency
);