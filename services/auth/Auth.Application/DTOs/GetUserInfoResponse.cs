namespace Auth.Application.DTOs;

/// <summary>
/// Represent a get user info response.
/// </summary>
public class GetUserInfoResponse
{
    /// <summary>
    /// User unique identifier.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// User username.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// User display name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Whether email is verified.
    /// </summary>
    public bool IsEmailVerified { get; set; }

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

    /// <summary>
    /// Last login date and time.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// User creation date and time.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}