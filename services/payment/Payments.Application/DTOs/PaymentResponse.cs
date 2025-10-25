namespace Payments.Application.DTOs;

public record PaymentResponse(
    long Id,
    long OrderId,
    string CustomerId,
    decimal Amount,
    string Currency,
    string Status,
    DateTime UpdatedAt
);