using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

/// <summary>
/// Interface for token services.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates JWT access token.
    /// </summary>
    /// <param name="user">User entity.</param>
    /// <param name="role">User role.</param>
    /// <returns>Access token string and expiry in milliseconds.</returns>
    (string, long) CreateAccessToken(User user, string role);
}