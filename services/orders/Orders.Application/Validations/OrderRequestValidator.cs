using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderRequest"/> model.
/// </summary>
public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.Items)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Consts.ErrorCode.ItemsEmpty);
    }
}