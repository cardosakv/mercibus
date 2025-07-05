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
    List<CategoryResponse> Categories,
    List<ProductImageResponse> Images,
    List<ProductAttributeResponse> Attributes,
    DateTime CreatedAt
);