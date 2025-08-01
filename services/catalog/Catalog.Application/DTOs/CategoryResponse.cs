namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a category response.
/// </summary>
public record CategoryResponse(
    long Id,
    long? ParentCategoryId,
    string Name,
    string? Description
);