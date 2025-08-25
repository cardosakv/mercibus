using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using MapsterMapper;
using Mercibus.Common.Constants;
using Mercibus.Common.Models;
using Mercibus.Common.Services;

namespace Catalog.Application.Services;

/// <summary>
///     Service for managing product reviews.
/// </summary>
public class ProductReviewService(IProductReviewRepository reviewRepository, IProductRepository productRepository, IMapper mapper, IAppDbContext dbContext)
    : BaseService, IProductReviewService
{
    public async Task<ServiceResult> GetProductReviewsAsync(long productId, ProductReviewQuery query, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        var reviews = await reviewRepository.GetProductReviewsAsync(productId, query, cancellationToken);

        var response = mapper.Map<List<ProductReviewResponse>>(reviews);
        return Success(response);
    }

    public async Task<ServiceResult> GetProductReviewByIdAsync(long productId, long reviewId, CancellationToken cancellationToken = default)
    {
        var review = await reviewRepository.GetProductReviewByIdAsync(reviewId, cancellationToken);
        if (review is null || review.ProductId != productId)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ReviewNotFound);
        }

        var response = mapper.Map<ProductReviewResponse>(review);
        return Success(response);
    }

    public async Task<ServiceResult> AddProductReviewAsync(long productId, string? userId, ProductReviewRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Error(ErrorType.AuthenticationError, ErrorCode.Unauthorized);
        }

        var product = await productRepository.GetProductByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ProductNotFound);
        }

        var entity = mapper.Map<ProductReview>(request);
        entity.ProductId = productId;
        entity.UserId = userId;

        var review = await reviewRepository.AddProductReviewAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ProductReviewResponse>(review);
        return Success(response);
    }

    public async Task<ServiceResult> UpdateProductReviewAsync(long productId, long reviewId, ProductReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = await reviewRepository.GetProductReviewByIdAsync(reviewId, cancellationToken);
        if (review is null || review.ProductId != productId)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ReviewNotFound);
        }

        mapper.Map(request, review);

        await reviewRepository.UpdateProductReviewAsync(review, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<ProductReviewResponse>(review);
        return Success(response);
    }

    public async Task<ServiceResult> DeleteProductReviewAsync(long productId, long reviewId, CancellationToken cancellationToken = default)
    {
        var review = await reviewRepository.GetProductReviewByIdAsync(reviewId, cancellationToken);
        if (review is null || review.ProductId != productId)
        {
            return Error(ErrorType.InvalidRequestError, Constants.ErrorCode.ReviewNotFound);
        }

        await reviewRepository.DeleteProductReviewAsync(review, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Success();
    }
}