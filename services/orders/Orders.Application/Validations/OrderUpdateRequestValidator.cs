using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderUpdateRequest"/> model.
/// </summary>
public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage(Constants.ErrorCode.InvalidOrderStatus);
    }
}