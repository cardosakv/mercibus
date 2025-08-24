using Catalog.Application.DTOs;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

/// <summary>
///     Repository implementation for managing product reviews.
/// </summary>
public class ProductReviewRepository(AppDbContext dbContext) : IProductReviewRepository
{
    public Task<List<ProductReview>> GetProductReviewsAsync(long productId, ProductReviewQuery query, CancellationToken cancellationToken = default)
    {
        var reviews = dbContext.ProductReviews.AsQueryable();

        if (!string.IsNullOrEmpty(query.UserId))
        {
            reviews = reviews.Where(r => r.UserId == query.UserId);
        }

        if (query.MinRating.HasValue)
        {
            reviews = reviews.Where(r => r.Rating >= query.MinRating.Value);
        }

        if (query.MaxRating.HasValue)
        {
            reviews = reviews.Where(r => r.Rating <= query.MaxRating.Value);
        }

        return reviews
            .Where(r => r.ProductId == productId)
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductReview> AddProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.ProductReviews.AddAsync(review, cancellationToken);
        return entry.Entity;
    }

    public Task<ProductReview?> GetProductReviewByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return dbContext.ProductReviews
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task UpdateProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
    {
        dbContext.ProductReviews.Update(review);
        return Task.CompletedTask;
    }

    public Task DeleteProductReviewAsync(ProductReview review, CancellationToken cancellationToken = default)
    {
        dbContext.ProductReviews.Remove(review);
        return Task.CompletedTask;
    }
}