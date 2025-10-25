using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
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
}