namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a category response.
/// </summary>
public record CategoryResponse(
    long Id,
    string Name,
    long? ParentCategoryId
);