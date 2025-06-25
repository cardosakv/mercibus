namespace AuthService.Application.DTOs
{
    /// <summary>
    /// Represents a request to register a new user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User email.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// User password in plain text.
        /// </summary>
        public string? Password { get; set; }
    }
}
