using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
{
    public ConfirmEmailQueryValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.UserIdRequired);

        RuleFor(x => x.Token)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ErrorCode.TokenRequired);
    }
}