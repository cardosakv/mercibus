using Auth.Application.Interfaces;
using FluentEmail.Core;

namespace Auth.Infrastructure.Services;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task<bool> SendEmailConfirmationLink(string email, string confirmationLink)
    {
        var template = Path.Combine(Directory.GetCurrentDirectory(), "Content", "ConfirmEmailTemplate.cshtml");
        var response = await fluentEmail
            .To(email)
            .Subject("Mercibus Email Confirmation")
            .UsingTemplateFromFile(template, confirmationLink)
            .SendAsync();

        return response.Successful;
    }

    public async Task<bool> SendPasswordResetLink(string email, string passwordResetLink)
    {
        var template = Path.Combine(Directory.GetCurrentDirectory(), "Content", "PasswordResetTemplate.cshtml");
        var response = await fluentEmail
            .To(email)
            .Subject("Mercibus Password Reset")
            .UsingTemplateFromFile(template, passwordResetLink)
            .SendAsync();

        return response.Successful;
    }
}