using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
        {
            var response = await productService.GetProductsAsync(query, cancellationToken);
            return Ok(response);
        }
    }
}