using Auth.Application.Common;
using Auth.Application.DTOs;

namespace Auth.Application.Interfaces.Services;

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
    /// Logouts a user.
    /// </summary>
    /// <param name="request">The logout request containing the refresh token.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> LogoutAsync(LogoutRequest request);

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
    Task<Response> SendConfirmationEmailAsync(SendConfirmationEmailRequest request);

    /// <summary>
    /// Verifies the user email based on the token sent to email.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="token">Verification token.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> ConfirmEmailAsync(string userId, string token);

    /// <summary>
    /// Sends a reset link to the specified user email.
    /// </summary>
    /// <param name="request">The request containing the user email address.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> ForgotPasswordAsync(ForgotPasswordRequest request);

    /// <summary>
    /// Resets the user password with a new one.
    /// </summary>
    /// <param name="request">The request containing the token and new password.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// Changes the current password of the user with a new one.
    /// </summary>
    /// <param name="request">The request containing the current and new password.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> ChangePasswordAsync(ChangePasswordRequest request);

    /// <summary>
    /// Gets the logged-in user's information.
    /// </summary>
    /// <param name="userId">The user ID of the user.</param>
    /// <returns><see cref="Response"/> with the user info.</returns>
    Task<Response> GetInfoAsync(string? userId);

    /// <summary>
    /// Updates the logged-in user's information.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="request">The request containing the user info.</param>
    /// <returns><see cref="Response"/> with a boolean value indicating whether the process was successful.</returns>
    Task<Response> UpdateInfoAsync(string? userId, UpdateUserInfoRequest request);
}