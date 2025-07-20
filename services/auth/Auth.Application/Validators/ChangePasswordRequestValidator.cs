using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for change password requests.
/// </summary>
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UserIdRequired);

        RuleFor(x => x.CurrentPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired)
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort)
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid);

        RuleFor(x => x.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired)
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort)
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid);
    }
}