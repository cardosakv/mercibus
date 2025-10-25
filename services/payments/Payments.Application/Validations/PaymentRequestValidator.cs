using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.Options;
using Payments.Application.Common;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Repositories;

namespace Payments.Application.Validations;

/// <summary>
/// Validator for PaymentRequest.
/// </summary>
public class PaymentRequestValidator: AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator(IPaymentRepository paymentRepository)
    {
        RuleFor(x => x.OrderId)
            .MustAsync(async (id, cancellationToken) => await paymentRepository.GetPaymentByOrderIdAsync(id, cancellationToken) != null)
            .WithMessage(Constants.ErrorCode.OrderNotFound);
    }
}