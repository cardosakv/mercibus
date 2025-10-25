using FluentValidation;
using Microsoft.Extensions.Configuration;
using Orders.Application.Common;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Services;

namespace Orders.Application.Validations;

/// <summary>
/// Validates the <see cref="OrderRequest"/> model.
/// </summary>
public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator(IProductReadService productReadService, IConfiguration configuration)
    {
        var supportedCurrencies = configuration.GetSection("SupportedCurrencies").Get<string[]>() ?? [];

        RuleFor(x => x.Currency)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.CurrencyRequired)
            .Must(currency => supportedCurrencies.Contains(currency)).WithMessage(Constants.ErrorCode.CurrencyInvalid);

        RuleFor(x => x.Items)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Constants.ErrorCode.ItemsEmpty);

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemRequestValidator(productReadService));
    }
}