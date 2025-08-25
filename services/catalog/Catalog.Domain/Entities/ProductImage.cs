using System.Text.Json.Serialization;

namespace Catalog.Domain.Entities;

/// <summary>
/// Represents an image associated with a product.
/// </summary>
public class ProductImage
{
    /// <summary>
    /// Unique identifier for the image.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Foreign key for the related product.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Product that this image belongs to.
    /// </summary>
    [JsonIgnore]
    public Product Product { get; set; } = null!;

    /// <summary>
    /// URL of the image.
    /// </summary>
    public required string ImageUrl { get; set; }

    /// <summary>
    /// Indicates whether this image is the primary image for the product.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Alternative text for accessibility or fallback.
    /// </summary>
    public string? AltText { get; set; }
}