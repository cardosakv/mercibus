namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to add a new product attribute.
/// </summary>
public record AddProductAttributeRequest(
    long ProductId,
    string Name,
    string Value,
    bool IsMandatory,
    string Type,
    string? Unit
);