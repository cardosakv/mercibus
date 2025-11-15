namespace Auth.Application.DTOs;

/// <summary>
/// Represents a forgot password request.
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Account username.
    /// </summary>
    public required string Username { get; set; }
}