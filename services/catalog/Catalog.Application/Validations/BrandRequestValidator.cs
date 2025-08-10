using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentValidation;

namespace Catalog.Application.Validations;

public class BrandRequestValidator : AbstractValidator<BrandRequest>
{
    public BrandRequestValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.NameRequired)
            .MaximumLength(Constants.CategoryValidation.MaxNameLength).WithMessage(Constants.ErrorCode.NameTooLong);

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(Constants.CategoryValidation.MaxDescriptionLength).WithMessage(Constants.ErrorCode.DescriptionTooLong);
    }
}