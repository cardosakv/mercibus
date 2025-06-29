namespace Auth.Application.DTOs;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Username handle.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// User password in plain text.
    /// </summary>
    public required string Password { get; set; }
}