using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentValidation;

namespace Catalog.Application.Validations;

public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(Constants.ProductValidation.MaxNameLength)
            .WithMessage($"Product name must not exceed {Constants.ProductValidation.MaxNameLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(Constants.ProductValidation.MaxDescriptionLength)
            .WithMessage($"Product description must not exceed {Constants.ProductValidation.MaxDescriptionLength} characters.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("Product SKU is required.")
            .MaximumLength(Constants.ProductValidation.MaxSkuLength)
            .WithMessage($"Product SKU must not exceed {Constants.ProductValidation.MaxSkuLength} characters.");
    }
}