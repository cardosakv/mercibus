namespace Messaging.Events;

/// <summary>
/// Event representing a successful payment.
/// </summary>
public record PaymentSucceeded(
    long PaymentId,
    long OrderId,
    string CustomerId,
    decimal TotalAmount,
    string Currency
);