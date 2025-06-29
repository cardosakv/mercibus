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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Street address.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// City address.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State address.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Country address.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Postal code address.
    /// </summary>
    public short PostalCode { get; set; }

    /// <summary>
    /// Created date and time of the user.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}