using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Services;
using Mercibus.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController(ICategoryService categoryService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] CategoryQuery query, CancellationToken cancellationToken)
    {
        var response = await categoryService.GetCategoriesAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await categoryService.AddCategoryAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCategoryByIdAsync(long id, CancellationToken cancellationToken)
    {
        var response = await categoryService.GetCategoryByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCategoryAsync(long id, [FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await categoryService.UpdateCategoryAsync(id, request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCategoryAsync(long id, CancellationToken cancellationToken)
    {
        var response = await categoryService.DeleteCategoryAsync(id, cancellationToken);
        return Ok(response);
    }
}