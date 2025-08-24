namespace Catalog.Application.DTOs;

/// <summary>
///     Represents a request to create or update a product review.
/// </summary>
public record ProductReviewRequest(
    int Rating,
    string? Comment = null
);