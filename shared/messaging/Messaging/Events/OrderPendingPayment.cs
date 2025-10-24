namespace Messaging.Events;

/// <summary>
/// Event representing that an order is pending payment.
/// </summary>
public record OrderPendingPayment(
    long OrderId,
    string CustomerId,
    decimal TotalAmount,
    string Currency
);