namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to add a new product.
/// </summary>
public record AddProductRequest(
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    int StockQuantity,
    long CategoryId,
    long BrandId
);