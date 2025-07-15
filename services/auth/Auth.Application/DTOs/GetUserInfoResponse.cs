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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether email is verified.
    /// </summary>
    public bool IsEmailVerified { get; set; }

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
    public short? PostalCode { get; set; }
}