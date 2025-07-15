using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.TokenRequired);
    }
}