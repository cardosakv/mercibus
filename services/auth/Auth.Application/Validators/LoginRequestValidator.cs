using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for login requests.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UsernameRequired)
            .MinimumLength(Constants.UserValidation.UsernameMinLength).WithMessage(ErrorCode.UsernameTooShort)
            .MaximumLength(Constants.UserValidation.UsernameMaxLength).WithMessage(ErrorCode.UsernameTooLong)
            .Matches(Constants.UserValidation.UsernamePattern).WithMessage(ErrorCode.UsernameInvalid);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired)
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort)
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid);
    }
}