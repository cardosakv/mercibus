using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for register requests.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UsernameRequired)
            .MinimumLength(Constants.UserValidation.UsernameMinLength).WithMessage(ErrorCode.UsernameTooShort)
            .MaximumLength(Constants.UserValidation.UsernameMaxLength).WithMessage(ErrorCode.UsernameTooLong)
            .Matches(Constants.UserValidation.UsernamePattern).WithMessage(ErrorCode.UsernameInvalid);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.EmailRequired)
            .EmailAddress().WithMessage(ErrorCode.EmailInvalid);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired)
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort)
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid);
    }
}