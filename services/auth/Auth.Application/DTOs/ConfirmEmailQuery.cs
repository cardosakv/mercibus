namespace Auth.Application.DTOs;

/// <summary>
/// Represents a query to confirm a user's email address.
/// </summary>
public class ConfirmEmailQuery
{
    /// <summary>
    /// User ID.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Email confirmation token.
    /// </summary>
    public required string Token { get; set; }
}