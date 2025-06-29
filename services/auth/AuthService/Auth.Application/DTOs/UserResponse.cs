namespace Auth.Application.DTOs;

/// <summary>
/// Represent a get or change user request.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// User display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

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
    public short? PostalCode { get; set; } = null;
}