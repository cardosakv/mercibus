namespace Catalog.Domain.Entities;

/// <summary>
/// Represents a product category, which can be nested hierarchically.
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Identifier of the parent category, if any.
    /// </summary>
    public long? ParentCategoryId { get; set; }

    /// <summary>
    /// Reference to the parent category entity.
    /// </summary>
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Name of the category.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the category.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date and time when the category was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// List of child categories under this category.
    /// </summary>
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();

    /// <summary>
    /// List of products under this category.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}