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
    /// <param name="confirmationLink">Confirmation link.</param>
    /// <returns>Boolean indicating success or failure.</returns>
    Task<bool> SendConfirmationLink(string email, string confirmationLink);
}