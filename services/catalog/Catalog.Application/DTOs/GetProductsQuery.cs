using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a query for retrieving products.
/// </summary>
public record GetProductsQuery(
    long? CategoryId,
    long? BrandId,
    decimal? MinPrice,
    decimal? MaxPrice,
    ProductStatus? Status,
    string? SortBy = "createdAt",
    string? SortDirection = "desc",
    int Page = 1,
    int PageSize = 20
);