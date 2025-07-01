namespace Auth.Application.Interfaces;

/// <summary>
/// Provides email services.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends the confirmation token to email address.
    /// </summary>
    /// <param name="email">Email address to send token.</param>
    /// <param name="confirmationLink">Confirmation link pointing to backend auth/confirm-email endpoint.</param>
    /// <returns>Boolean indicating success or failure.</returns>
    Task<bool> SendEmailConfirmationLink(string email, string confirmationLink);

    /// <summary>
    /// Sends the password reset link to email address.
    /// </summary>
    /// <param name="email">Email address to send token.</param>
    /// <param name="passwordResetLink">Reset link from the frontend app.</param>
    /// <returns>Boolean indicating success or failure.</returns>
    Task<bool> SendPasswordResetLink(string email, string passwordResetLink);
}