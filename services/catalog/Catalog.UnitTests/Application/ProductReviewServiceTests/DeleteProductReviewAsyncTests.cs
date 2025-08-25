using Catalog.Application.Common;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

public class DeleteProductReviewAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenReviewIsDeleted()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;

        var existingReview = new ProductReview
        {
            Id = reviewId,
            ProductId = productId,
            UserId = "user1",
            Rating = 5,
            Comment = "Review to delete",
            CreatedAt = DateTime.UtcNow
        };

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReview);
        ProductReviewRepositoryMock
            .Setup(x => x.DeleteProductReviewAsync(existingReview, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await ProductReviewService.DeleteProductReviewAsync(productId, reviewId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
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
        var result = await ProductReviewService.DeleteProductReviewAsync(productId, reviewId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }

    [Fact]
    public async Task Error_WhenReviewBelongsToDifferentProduct()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;

        var review = new ProductReview
        {
            Id = reviewId,
            ProductId = 2L, // belongs to another product
            UserId = "user1",
            Rating = 2,
            Comment = "Wrong product review",
            CreatedAt = DateTime.UtcNow
        };

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(review);

        // Act
        var result = await ProductReviewService.DeleteProductReviewAsync(productId, reviewId, CancellationToken.None);

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
        Func<Task> act = async () => await ProductReviewService.DeleteProductReviewAsync(productId, reviewId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Repository failure");
    }

    [Fact]
    public async Task Throws_WhenSaveChangesFails()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;

        var existingReview = new ProductReview
        {
            Id = reviewId,
            ProductId = productId,
            UserId = "user1",
            Rating = 4,
            Comment = "Will fail on save",
            CreatedAt = DateTime.UtcNow
        };

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReview);
        ProductReviewRepositoryMock
            .Setup(x => x.DeleteProductReviewAsync(existingReview, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database save failed"));

        // Act
        Func<Task> act = async () => await ProductReviewService.DeleteProductReviewAsync(productId, reviewId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database save failed");
    }
}