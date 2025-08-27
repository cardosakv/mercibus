namespace Catalog.Application.DTOs;

/// <summary>
///     Represents a query for retrieving product reviews.
/// </summary>
public record ProductReviewQuery(
    string? UserId = null,
    int? MinRating = null,
    int? MaxRating = null,
    int Page = 1,
    int PageSize = 20
);