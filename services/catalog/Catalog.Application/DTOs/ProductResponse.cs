namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a product response in the catalog system.
/// </summary>
public record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string Sku,
    string Status,
    int StockQuantity,
    BrandResponse Brand,
    CategoryResponse Category,
    List<ProductImageResponse> Images,
    List<ProductAttributeResponse> Attributes,
    DateTime CreatedAt
);