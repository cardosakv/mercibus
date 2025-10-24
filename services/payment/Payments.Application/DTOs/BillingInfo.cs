namespace Payments.Application.DTOs;

/// <summary>
/// Represents billing information for a payment.
/// </summary>
public record BillingInfo(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string StreetLine1,
    string StreetLine2,
    string City,
    string State,
    string PostalCode,
    string Country
);