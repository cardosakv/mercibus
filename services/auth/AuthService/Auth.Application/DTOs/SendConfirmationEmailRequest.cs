namespace Auth.Application.DTOs
{
    /// <summary>
    /// Represents a request to send a confirmation to the email.
    /// </summary>
    public class SendConfirmationEmailRequest
    {
        /// <summary>
        /// Email address to send confirmation.
        /// </summary>
        public required string Email { get; set; }
    }
}