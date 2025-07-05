namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a product image response.
/// </summary>
public record ProductImageResponse(
    long Id,
    long ImageUrl,
    bool IsPrimary,
    string? AltText
);