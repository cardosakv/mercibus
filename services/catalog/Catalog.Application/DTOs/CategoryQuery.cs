namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a query for retrieving categories.
/// </summary>
public record CategoryQuery(
    long? ParentCategoryId = null,
    int Page = 1,
    int PageSize = 20
);