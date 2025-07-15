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
            .NotEmpty().WithMessage(ErrorCode.UsernameRequired.GetEnumMemberValue())
            .MinimumLength(Constants.UserValidation.UsernameMinLength).WithMessage(ErrorCode.UsernameTooShort.GetEnumMemberValue())
            .MaximumLength(Constants.UserValidation.UsernameMaxLength).WithMessage(ErrorCode.UsernameTooLong.GetEnumMemberValue())
            .Matches(Constants.UserValidation.UsernamePattern).WithMessage(ErrorCode.UsernameInvalid.GetEnumMemberValue());

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.EmailRequired.GetEnumMemberValue())
            .EmailAddress().WithMessage(ErrorCode.EmailInvalid.GetEnumMemberValue());

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired.GetEnumMemberValue())
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort.GetEnumMemberValue())
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid.GetEnumMemberValue());
    }
}