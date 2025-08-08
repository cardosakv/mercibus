using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/brands")]
[ApiController]
public class BrandController(IBrandService brandService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetBrandsAsync([FromQuery] BrandQuery query, CancellationToken cancellationToken)
    {
        var response = await brandService.GetBrandsAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrandAsync([FromBody] BrandRequest request, CancellationToken cancellationToken)
    {
        var response = await brandService.AddBrandAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBrandByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await brandService.GetBrandByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateBrandAsync(long id, [FromBody] BrandRequest request, CancellationToken cancellationToken)
    {
        var response = await brandService.UpdateBrandAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteBrandAsync(long id, CancellationToken cancellationToken)
    {
        var response = await brandService.DeleteBrandAsync(id, cancellationToken);
        return Ok(response);
    }
}