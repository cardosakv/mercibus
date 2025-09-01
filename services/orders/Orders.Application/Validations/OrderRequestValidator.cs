using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Services;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderRequest"/> model.
/// </summary>
public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator(IProductReadService productReadService)
    {
        RuleFor(x => x.Items)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ItemsEmpty);

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemRequestValidator(productReadService));
    }
}