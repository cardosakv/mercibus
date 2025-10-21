using FluentValidation;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Domain.Enums;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderUpdateRequest"/> model.
/// </summary>
public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => Enum.TryParse(typeof(OrderStatus), status, true, out _))
            .WithMessage(Constants.ErrorCode.InvalidOrderStatus);
    }
}