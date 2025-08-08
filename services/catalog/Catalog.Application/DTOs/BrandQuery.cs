namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a query for retrieving brands.
/// </summary>
public record BrandQuery(
    string? Region = null,
    int Page = 1,
    int PageSize = 20
);