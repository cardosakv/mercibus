using System.Security.Claims;
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await orderService.AddAsync(userId, request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetOrderByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await orderService.GetByIdAsync(id, cancellationToken);
        return Ok(response);
    }
}