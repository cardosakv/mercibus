using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService productService) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsAsync([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductsAsync(query, cancellationToken);
        return HandleGet(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProductAsync([FromBody] AddProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.AddProductAsync(request, cancellationToken);
        return HandlePost(response);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductByIdAsync(id, cancellationToken);
        return HandleGet(response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductAsync(long id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.UpdateProductAsync(id, request, cancellationToken);
        return HandlePutOrDelete(response);
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(StandardResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProductAsync(long id, CancellationToken cancellationToken)
    {
        var response = await productService.DeleteProductAsync(id, cancellationToken);
        return HandlePutOrDelete(response);
    }
}