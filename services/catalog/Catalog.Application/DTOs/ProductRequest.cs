namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to update an existing product.
/// </summary>
public record ProductRequest(
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    int StockQuantity,
    Dictionary<string, string> Attributes,
    long CategoryId,
    long BrandId
);