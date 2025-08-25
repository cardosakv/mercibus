using System.Security.Claims;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService productService, IProductImageService productImageService, IProductReviewService productReviewService) : BaseController
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

    [HttpGet("{productId:long}")]
    public async Task<IActionResult> GetProductByIdAsync(long productId, CancellationToken cancellationToken)
    {
        var response = await productService.GetProductByIdAsync(productId, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{productId:long}")]
    public async Task<IActionResult> UpdateProductAsync(long productId, [FromBody] ProductRequest request, CancellationToken cancellationToken)
    {
        var response = await productService.UpdateProductAsync(productId, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{productId:long}")]
    public async Task<IActionResult> DeleteProductAsync(long productId, CancellationToken cancellationToken)
    {
        var response = await productService.DeleteProductAsync(productId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{productId:long}/images")]
    public async Task<IActionResult> AddProductImageAsync(long productId, [FromForm] ProductImageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await productImageService.AddProductImageAsync(productId, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{productId:long}/images/{imageId:long}")]
    public async Task<IActionResult> DeleteProductImageAsync(long productId, long imageId, CancellationToken cancellationToken)
    {
        var response = await productImageService.DeleteProductImageAsync(productId, imageId, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{productId:long}/reviews")]
    public async Task<IActionResult> GetProductReviewsAsync(long productId, [FromQuery] ProductReviewQuery query, CancellationToken cancellationToken)
    {
        var response = await productReviewService.GetProductReviewsAsync(productId, query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{productId:long}/reviews/{reviewId:long}")]
    public async Task<IActionResult> GetProductReviewByIdAsync(long productId, long reviewId, CancellationToken cancellationToken)
    {
        var response = await productReviewService.GetProductReviewByIdAsync(productId, reviewId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{productId:long}/reviews")]
    public async Task<IActionResult> AddProductReviewAsync(long productId, [FromBody] ProductReviewRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await productReviewService.AddProductReviewAsync(productId, userId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{productId:long}/reviews/{reviewId:long}")]
    public async Task<IActionResult> UpdateProductReviewAsync(long productId, long reviewId, [FromBody] ProductReviewRequest request, CancellationToken cancellationToken)
    {
        var response = await productReviewService.UpdateProductReviewAsync(productId, reviewId, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{productId:long}/reviews/{reviewId:long}")]
    public async Task<IActionResult> DeleteProductReviewAsync(long productId, long reviewId, CancellationToken cancellationToken)
    {
        var response = await productReviewService.DeleteProductReviewAsync(productId, reviewId, cancellationToken);
        return Ok(response);
    }
}