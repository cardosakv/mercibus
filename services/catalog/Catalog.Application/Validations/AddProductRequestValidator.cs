using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using FluentValidation;

namespace Catalog.Application.Validations;

public class AddProductRequestValidator : AbstractValidator<AddProductRequest>
{
    public AddProductRequestValidator(IAppDbContext dbContext)
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
    }
}