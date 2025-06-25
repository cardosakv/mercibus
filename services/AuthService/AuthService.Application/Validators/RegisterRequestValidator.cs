using AuthService.Application.DTOs;
using FluentValidation;

namespace AuthService.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="RegisterRequest"/>.
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("'Name' is required.")
                .MinimumLength(2).WithMessage("'Name' must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("'Name' must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("'Email' is required.")
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("'Password' is required.")
                .MinimumLength(8).WithMessage("'Password' must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("'Password' must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("'Password' must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("'Password' must contain at least one digit.");
        }
    }
}
