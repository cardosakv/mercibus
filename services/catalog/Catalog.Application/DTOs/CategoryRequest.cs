namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to create or update a category.
/// </summary>
public record CategoryRequest(
    long? ParentCategoryId,
    string Name,
    string? Description
);