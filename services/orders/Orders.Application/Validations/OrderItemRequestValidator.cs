using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;

namespace Orders.Application.Validations;

public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ProductIdRequired)
            .Must((id) => true).WithMessage(Constants.ErrorCode.ProductNotFound);
        
        RuleFor(x => x.ProductName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ProductNameRequired);

        RuleFor(x => x.Quantity)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.QuantityRequired)
            .GreaterThan(0).WithMessage(Constants.ErrorCode.QuantityInvalid);
    }
}