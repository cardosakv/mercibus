namespace Auth.Application.DTOs;

/// <summary>
/// Represents a forgot password request.
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Email address to send reset link.
    /// </summary>
    public required string Email { get; set; }
}