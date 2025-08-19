using Microsoft.AspNetCore.Http;

namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to add a new product image.
/// </summary>
public record ProductImageRequest(
    IFormFile Image,
    bool IsPrimary = false,
    string? AltText = null
);