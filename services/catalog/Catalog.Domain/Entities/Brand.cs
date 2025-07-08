namespace Catalog.Domain.Entities;

/// <summary>
/// Represents a product brand or manufacturer.
/// </summary>
public class Brand
{
    /// <summary>
    /// Unique identifier for the brand.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the brand.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the brand.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// URL to the brand's logo.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Region where the brand is based or operates.
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    /// Website URL of the brand.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Additional information about the brand.
    /// </summary>
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Date and time created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// List of products under this brand.
    /// </summary>
    public ICollection<Product>? Products { get; set; }
}