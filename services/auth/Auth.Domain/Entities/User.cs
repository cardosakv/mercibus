using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities;

/// <summary>
/// Represents a user in the authentication system.
/// </summary>
public class User : IdentityUser
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
    /// Created date and time of the user.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}