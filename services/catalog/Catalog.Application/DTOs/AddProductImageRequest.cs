namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to add a new product image.
/// </summary>
public record AddProductImageRequest(
    long ProductId,
    string ImageUrl,
    bool IsPrimary = false,
    string? AltText = null
);