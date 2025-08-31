using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Interfaces.Services;

namespace Orders.Api.Controllers;

[Route("api/orders")]
public class OrderController(IOrderService orderService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddOrderAsync([FromBody] OrderRequest request, CancellationToken cancellationToken)
    {
        var response = await orderService.AddAsync(request, cancellationToken);
        return Ok(response);
    }
}