using FluentValidation;
using Payments.Application.Common;
using Payments.Application.DTOs;

namespace Payments.Application.Validations;

/// <summary>
/// Validator for billing request.
/// </summary>
public class BillingRequestValidator : AbstractValidator<BillingRequest>
{
    public BillingRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Constants.ErrorCode.FirstNameRequired);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Constants.ErrorCode.LastNameRequired);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.EmailRequired)
            .EmailAddress().WithMessage(Constants.ErrorCode.EmailInvalid);

        RuleFor(x => x.StreetLine1)
            .NotEmpty().WithMessage(Constants.ErrorCode.StreetLine1Required);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage(Constants.ErrorCode.CityRequired);

        RuleFor(x => x.State)
            .NotEmpty().WithMessage(Constants.ErrorCode.StateRequired);

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage(Constants.ErrorCode.PostalCodeRequired);

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage(Constants.ErrorCode.CountryRequired);
    }
}