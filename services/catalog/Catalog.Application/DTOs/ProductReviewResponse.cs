namespace Catalog.Application.DTOs;

/// <summary>
///     Represents a product review response.
/// </summary>
public record ProductReviewResponse(
    long Id,
    long ProductId,
    string UserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt
);