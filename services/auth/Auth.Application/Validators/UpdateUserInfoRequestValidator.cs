using Auth.Application.Common;
using Auth.Application.DTOs;
using FluentValidation;

namespace Auth.Application.Validators;

/// <summary>
/// Validator for update user info requests.
/// </summary>
public class UpdateUserInfoRequestValidator : AbstractValidator<UpdateUserInfoRequest>
{
    public UpdateUserInfoRequestValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(Constants.UserValidation.NameMinLength).WithMessage(ErrorCode.NameTooShort)
            .MaximumLength(Constants.UserValidation.NameMaxLength).WithMessage(ErrorCode.NameTooLong);
    }
}