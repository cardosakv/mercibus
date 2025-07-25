namespace Auth.Application.DTOs;

/// <summary>
/// Authentication token generated by JWT.
/// </summary>
public class AuthTokenResponse
{
    /// <summary>
    /// Token type.
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Short-live token.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Expiration time of the access token in milliseconds.
    /// </summary>
    public required long ExpiresIn { get; set; }

    /// <summary>
    /// Persisted refresh token.
    /// </summary>
    public required string RefreshToken { get; set; }
}