namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to update an existing product.
/// </summary>
public record UpdateProductRequest(
    long Id,
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    int StockQuantity,
    string Status,
    long CategoryId,
    long BrandId
);
