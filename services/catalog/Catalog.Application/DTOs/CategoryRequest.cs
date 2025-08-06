namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to update an existing category.
/// </summary>
public record CategoryRequest(
    long? ParentCategoryId,
    string Name,
    string? Description
);