namespace Auth.Domain.Entities;

/// <summary>
/// Refresh token entity.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Hashed token value.
    /// </summary>
    public required string TokenHash { get; set; }

    /// <summary>
    /// Owner of the refresh token.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Token expiry.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Whether token is already invalidated.
    /// </summary>
    public bool IsRevoked { get; set; }
}