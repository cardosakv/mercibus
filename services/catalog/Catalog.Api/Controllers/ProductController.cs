using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService productService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductsAsync(query, cancellationToken);
        return HandleGet(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductByIdAsync(id, cancellationToken);
        return HandleGet(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.AddProductAsync(request, cancellationToken);
        return HandlePost(response);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProductAsync(long id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.UpdateProductAsync(id, request, cancellationToken);
        return HandlePutOrDelete(response);
    }
}