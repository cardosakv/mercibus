using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

public class UpdateProductReviewAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenReviewIsUpdated()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 10L;
        var request = new ProductReviewRequest(Rating: 4, Comment: "Updated review text");

        var existingReview = new ProductReview
        {
            Id = reviewId,
            ProductId = productId,
            UserId = "user1",
            Rating = 5,
            Comment = "Old review",
            CreatedAt = DateTime.UtcNow
        };

        var updatedReview = new ProductReview
        {
            Id = reviewId,
            ProductId = productId,
            UserId = "user1",
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = existingReview.CreatedAt
        };

        var response = new ProductReviewResponse(
            updatedReview.Id,
            updatedReview.ProductId,
            updatedReview.UserId,
            updatedReview.Rating,
            updatedReview.Comment,
            updatedReview.CreatedAt
        );

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReview);
        MapperMock
            .Setup(x => x.Map(request, existingReview))
            .Callback<ProductReviewRequest, ProductReview>((req, r) =>
            {
                r.Rating = req.Rating;
                r.Comment = req.Comment;
            });
        ProductReviewRepositoryMock
            .Setup(x => x.UpdateProductReviewAsync(existingReview, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        DbContextMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock
            .Setup(x => x.Map<ProductReviewResponse>(existingReview))
            .Returns(response);

        // Act
        var result = await ProductReviewService.UpdateProductReviewAsync(productId, reviewId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Error_WhenReviewDoesNotExist()
    {
        // Arrange
        var productId = 1L;
        var reviewId = 99L;
        var request = new ProductReviewRequest(Rating: 3, Comment: "Non-existent review");

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductReview?)null);

        // Act
        var result = await ProductReviewService.UpdateProductReviewAsync(productId, reviewId, request, CancellationToken.None);

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
        var request = new ProductReviewRequest(Rating: 4, Comment: "Wrong product test");

        var existingReview = new ProductReview
        {
            Id = reviewId,
            ProductId = 2L, // different productId
            UserId = "user1",
            Rating = 2,
            Comment = "Wrong product",
            CreatedAt = DateTime.UtcNow
        };

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingReview);

        // Act
        var result = await ProductReviewService.UpdateProductReviewAsync(productId, reviewId, request, CancellationToken.None);

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
        var request = new ProductReviewRequest(Rating: 5, Comment: "Repository failure test");

        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Repository failure"));

        // Act
        Func<Task> act = async () => await ProductReviewService.UpdateProductReviewAsync(productId, reviewId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Repository failure");
    }
}