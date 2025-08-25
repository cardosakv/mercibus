using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

public class GetProductReviewByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenReviewExists()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;

        var review = new ProductReview
        {
            Id = reviewId,
            ProductId = productId,
            UserId = "user1",
            Rating = 5,
            Comment = "Great product!",
            CreatedAt = DateTime.UtcNow
        };

        var expectedResponse = new ProductReviewResponse(
            reviewId,
            productId,
            UserId: "user1",
            Rating: 5,
            Comment: "Great product!",
            review.CreatedAt
        );

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);
        MapperMock
            .Setup(x => x.Map<ProductReviewResponse>(review))
            .Returns(expectedResponse);

        // Act
        var result = await ProductReviewService.GetProductReviewByIdAsync(productId, reviewId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Error_WhenReviewDoesNotExist()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 99L;

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductReview?)null);

        // Act
        var result = await ProductReviewService.GetProductReviewByIdAsync(productId, reviewId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }

    [Fact]
    public async Task Error_WhenReviewDoesNotBelongToProduct()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 20L;

        var review = new ProductReview
        {
            Id = reviewId,
            ProductId = 2L, // different productId
            UserId = "user1",
            Rating = 4,
            Comment = "Wrong product",
            CreatedAt = DateTime.UtcNow
        };

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await ProductReviewService.GetProductReviewByIdAsync(productId, reviewId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrowsException()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Repository failure"));

        // Act
        Func<Task> act = async () => await ProductReviewService.GetProductReviewByIdAsync(productId, reviewId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Repository failure");
    }
}