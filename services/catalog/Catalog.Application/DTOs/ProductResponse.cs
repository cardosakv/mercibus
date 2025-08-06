namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a product response in the catalog system.
/// </summary>
public record ProductResponse(
    long Id,
    string Name,
    string Description,
    decimal Price,
    string Sku,
    int StockQuantity,
    long BrandId,
    long CategoryId,
    List<ProductImageResponse> Images,
    List<ProductAttributeResponse> Attributes,
    DateTime CreatedAt
);