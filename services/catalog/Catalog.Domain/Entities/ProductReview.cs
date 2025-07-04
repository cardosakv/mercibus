namespace Catalog.Domain.Entities;

/// <summary>
/// Represents a customer review for a product.
/// </summary>
public class ProductReview
{
    /// <summary>
    /// Unique identifier for the review.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Foreign key for the reviewed product.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Product that was reviewed.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Identifier of the user who submitted the review.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Rating given to the product.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Optional text comment provided by the user.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Date and time when the review was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}