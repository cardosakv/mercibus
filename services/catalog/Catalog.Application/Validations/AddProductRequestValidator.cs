using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Enums;
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

        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse<ProductStatus>(status, ignoreCase: true, out _))
            .WithMessage("Status must be one of the defined values: " + $"{string.Join(", ", Enum.GetNames(typeof(ProductStatus)))}.");
    }
}