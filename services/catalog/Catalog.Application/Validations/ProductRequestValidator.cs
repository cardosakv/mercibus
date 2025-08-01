using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using FluentValidation;

namespace Catalog.Application.Validations;

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.NameRequired)
            .MaximumLength(Constants.ProductValidation.MaxNameLength).WithMessage(Constants.ErrorCode.NameTooLong);

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(Constants.ProductValidation.MaxDescriptionLength).WithMessage(Constants.ErrorCode.DescriptionTooLong);

        RuleFor(x => x.Sku)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.SkuRequired);

        RuleFor(x => x.Price)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.PriceRequired);

        RuleFor(x => x.StockQuantity)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.StockQuantityRequired);

        RuleFor(x => x.CategoryId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, cancellationToken) => await categoryRepository.DoesCategoryExistsAsync(id, cancellationToken))
            .WithMessage(Constants.ErrorCode.CategoryNotFound);
    }
}