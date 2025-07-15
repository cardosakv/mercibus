using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for send email confirmation requests.
/// </summary>
public class SendConfirmationEmailRequestValidator : AbstractValidator<SendConfirmationEmailRequest>
{
    public SendConfirmationEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.EmailRequired.GetEnumMemberValue())
            .EmailAddress().WithMessage(ErrorCode.EmailInvalid.GetEnumMemberValue());
    }
}