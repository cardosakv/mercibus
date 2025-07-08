namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a product attribute response.
/// </summary>
public record ProductAttributeResponse(
    long Id,
    string Name,
    string Value,
    string Type,
    string? Unit
);