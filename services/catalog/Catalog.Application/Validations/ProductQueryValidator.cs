using Catalog.Application.Common;
using Catalog.Application.DTOs;
using FluentValidation;

namespace Catalog.Application.Validations;

public class ProductQueryValidator : AbstractValidator<ProductQuery>
{
    public ProductQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0).WithMessage(Constants.ErrorCode.PriceInvalid);
    }
}