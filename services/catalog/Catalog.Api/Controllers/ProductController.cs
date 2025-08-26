using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/products")]
[Authorize]
[ApiController]
public class ProductController(IProductService productService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductQuery query, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductsAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductAsync([FromBody] ProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.AddProductAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProductAsync(long id, [FromBody] ProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.UpdateProductAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProductAsync(long id, CancellationToken cancellationToken)
    {
        var response = await productService.DeleteProductAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{productId:long}/images")]
    public async Task<IActionResult> AddProductImageAsync(long productId, [FromForm] ProductImageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await productService.AddProductImageAsync(productId, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{productId:long}/images/{imageId:long}")]
    public async Task<IActionResult> DeleteProductImageAsync(long productId, long imageId, CancellationToken cancellationToken)
    {
        var response = await productService.DeleteProductImageAsync(productId, imageId, cancellationToken);
        return Ok(response);
    }
}