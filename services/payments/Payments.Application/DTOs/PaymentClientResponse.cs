namespace Payments.Application.DTOs;

/// <summary>
/// Represents the response from a payment client.
/// </summary>
public record PaymentClientResponse(
    string PaymentSessionId,
    string ReferenceId,
    string CustomerId,
    string Status,
    string PaymentLinkUrl,
    string PaymentTokenId
);