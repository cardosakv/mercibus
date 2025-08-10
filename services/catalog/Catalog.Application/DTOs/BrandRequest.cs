namespace Catalog.Application.DTOs;

/// <summary>
/// Represents a request to create or update a brand.
/// </summary>
public record BrandRequest(
    string Name,
    string? Description = null,
    string? LogoUrl = null,
    string? Region = null,
    string? Website = null,
    string? AdditionalInfo = null
);