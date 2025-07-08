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
    string Status,
    long CategoryId,
    long BrandId,
    List<AddProductImageRequest> Images,
    List<AddProductAttributeRequest> Attributes
);