namespace Auth.Application.DTOs;

/// <summary>
/// Represents a request to logout a user.
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// User refresh token.
    /// </summary>
    public required string RefreshToken { get; set; }
}