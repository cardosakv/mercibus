using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities;

/// <summary>
/// Represents a product listed in the catalog.
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Product SKU.
    /// </summary>
    public required string Sku { get; set; }

    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Availability status of the product.
    /// </summary>
    public ProductStatus Status { get; set; }

    /// <summary>
    /// Quantity of the product in stock.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Foreign key for the category the product belongs to.
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Category associated with the product.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Foreign key for the brand of the product.
    /// </summary>
    public long BrandId { get; set; }

    /// <summary>
    /// Brand associated with the product.
    /// </summary>
    public Brand Brand { get; set; } = null!;
    
    /// <summary>
    /// Date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Collection of images associated with the product.
    /// </summary>
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    /// <summary>
    /// Collection of attributes describing the product.
    /// </summary>
    public ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();
}