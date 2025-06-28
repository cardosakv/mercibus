using Auth.Application.Interfaces;
using FluentEmail.Core;

namespace Auth.Infrastructure.Services
{
    public class EmailService(IFluentEmail fluentEmail) : IEmailService
    {
        public async Task<bool> SendConfirmationLink(string email, string confirmationLink)
        {
            var template = Directory.GetCurrentDirectory() + @"\Content\EmailTemplate.cshtml";
            var response = await fluentEmail
                .To(email)
                .Subject("Mercibus Confirmation")
                .UsingTemplateFromFile(template, confirmationLink)
                .SendAsync();

            return response.Successful;
        }
    }
}