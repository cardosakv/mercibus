using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Services;

namespace Payments.Api.Controllers;

[Route("api/payments")]
public class PaymentController(IPaymentService paymentService) : BaseController
{
    [HttpGet("{paymentId:long}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentById(long paymentId, CancellationToken cancellationToken)
    {
        var response = await paymentService.GetPaymentByIdAsync(paymentId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("initiate")]
    [Authorize]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await paymentService.InitiatePaymentAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> ProcessPaymentWebhook([FromBody] PaymentWebhookRequest request, CancellationToken cancellationToken)
    {
        var response = await paymentService.ProcessPaymentWebhookAsync(request, cancellationToken);
        return Ok(response);
    }
}