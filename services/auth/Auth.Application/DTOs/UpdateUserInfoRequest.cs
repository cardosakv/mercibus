namespace Auth.Application.DTOs;

/// <summary>
/// Represents an update user info request.
/// </summary>
public class UpdateUserInfoRequest
{
    /// <summary>
    /// User display name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Street address.
    /// </summary>
    public string? Street { get; set; }

    /// <summary>
    /// City address.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// State address.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Country address.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Postal code address.
    /// </summary>
    public short? PostalCode { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Profile image URL.
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}