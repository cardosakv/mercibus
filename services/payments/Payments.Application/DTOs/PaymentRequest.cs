namespace Payments.Application.DTOs;

/// <summary>
/// Represents a payment request.
/// </summary>
public record PaymentRequest(
    long OrderId,
    BillingRequest BillingRequest
);