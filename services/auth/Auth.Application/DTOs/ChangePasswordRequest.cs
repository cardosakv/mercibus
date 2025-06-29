namespace Auth.Application.DTOs;

/// <summary>
/// Represents a change password request.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// User ID.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Existing current password.
    /// </summary>
    public required string CurrentPassword { get; set; }

    /// <summary>
    /// New password.
    /// </summary>
    public required string NewPassword { get; set; }
}