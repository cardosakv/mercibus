namespace Payments.Application.DTOs;

/// <summary>
/// Represents a request to the payment client.
/// </summary>
public record PaymentClientRequest(
    string ReferenceId,
    decimal Amount,
    string Currency,
    string Country,
    string SessionType,
    string Mode
);