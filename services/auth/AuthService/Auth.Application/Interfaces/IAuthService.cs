using Auth.Application.Common;
using Auth.Application.DTOs;

namespace Auth.Application.Interfaces;

/// <summary>
/// Interface for authentication services.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing user email and password.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Logins a user.
    /// </summary>
    /// <param name="request">The registration request containing username and password.</param>
    /// <returns><see cref="Response"/> with the generated token.</returns>
    Task<Response> LoginAsync(LoginRequest request);

    /// <summary>
    /// Provides new tokens using the refresh token.
    /// </summary>
    /// <param name="request">The refresh request containing refresh token string.</param>
    /// <returns><see cref="Response"/> with the generated token.</returns>
    Task<Response> RefreshTokenAsync(RefreshRequest request);

    /// <summary>
    /// Sends a confirmation to the specified user email.
    /// </summary>
    /// <param name="request">The request containing the user email address.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> SendConfirmationEmail(SendConfirmationEmailRequest request);

    /// <summary>
    /// Verifies the user email based on the token sent to email.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="token">Verification token.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> ConfirmEmail(string userId, string token);
}