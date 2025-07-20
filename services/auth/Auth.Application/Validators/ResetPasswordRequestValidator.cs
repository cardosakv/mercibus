using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UserIdRequired);

        RuleFor(x => x.Token)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.TokenRequired);

        RuleFor(x => x.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.PasswordRequired)
            .MinimumLength(Constants.UserValidation.PasswordMinLength).WithMessage(ErrorCode.PasswordTooShort)
            .Matches(Constants.UserValidation.PasswordPattern).WithMessage(ErrorCode.PasswordInvalid);
    }
}