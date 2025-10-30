namespace Payments.Application.DTOs;

/// <summary>
/// Represents an error response from the payment client.
/// </summary>
public record PaymentClientError(
    string ErrorCode,
    string Message
);