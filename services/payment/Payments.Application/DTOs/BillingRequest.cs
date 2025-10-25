namespace Payments.Application.DTOs;

/// <summary>
/// Represents billing information for a payment.
/// </summary>
public record BillingRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string StreetLine1,
    string? StreetLine2,
    string City,
    string State,
    int PostalCode,
    string Country
);