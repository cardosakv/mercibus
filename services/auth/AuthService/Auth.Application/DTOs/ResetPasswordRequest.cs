namespace Auth.Application.DTOs;

/// <summary>
/// Represents a reset password request.
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// User ID.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Password reset token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// New password.
    /// </summary>
    public required string NewPassword { get; set; }
}