using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Services;

namespace Payments.Api.Controllers;

[Route("api/payments")]
public class PaymentController(IPaymentService paymentService) : BaseController
{
    [HttpGet("{paymentId:long}")]
    public async Task<IActionResult> GetPaymentById(long paymentId, CancellationToken cancellationToken)
    {
        var response = await paymentService.GetPaymentByIdAsync(paymentId, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var response = await paymentService.InitiatePaymentAsync(request, cancellationToken);
        return Ok(response);
    }
}