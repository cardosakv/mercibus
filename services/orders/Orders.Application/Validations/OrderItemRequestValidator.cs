using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Services;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderItemRequest"/> model.
/// </summary>
public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator(IProductReadService productReadService)
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ProductIdRequired)
            .MustAsync(async (id, cancellationToken) => await productReadService.ExistsAsync(id, cancellationToken))
            .WithMessage(Constants.ErrorCode.ProductNotFound);

        RuleFor(x => x.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ProductNameRequired);

        RuleFor(x => x.Quantity)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.QuantityRequired)
            .GreaterThan(0).WithMessage(Constants.ErrorCode.QuantityInvalid);

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage(Constants.ErrorCode.PriceRequired)
            .GreaterThan(0).WithMessage(Constants.ErrorCode.PriceInvalid);
    }
}