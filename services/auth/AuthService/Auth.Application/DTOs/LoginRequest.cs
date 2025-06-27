namespace Auth.Application.DTOs
{
    /// <summary>
    /// Represents a request to login a user.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username handle.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// User password in plain text.
        /// </summary>
        public required string Password { get; set; }
    }
}
