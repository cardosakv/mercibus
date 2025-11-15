using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for forgot password requests.
/// </summary>
public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UsernameRequired)
            .MinimumLength(Constants.UserValidation.UsernameMinLength).WithMessage(ErrorCode.UsernameTooShort)
            .MaximumLength(Constants.UserValidation.UsernameMaxLength).WithMessage(ErrorCode.UsernameTooLong)
            .Matches(Constants.UserValidation.UsernamePattern).WithMessage(ErrorCode.UsernameInvalid);
    }
}