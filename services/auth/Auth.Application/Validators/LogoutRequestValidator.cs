using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for logout requests.
/// </summary>
public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.TokenRequired);
    }
}