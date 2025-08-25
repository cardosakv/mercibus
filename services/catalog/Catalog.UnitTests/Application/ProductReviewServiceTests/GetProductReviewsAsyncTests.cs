using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

public class GetProductReviewsAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenReviewsExist()
    {
        // Arrange
        var productId = 1L;
        var query = new ProductReviewQuery();

        var product = new Product
        {
            Id = productId,
            Name = "Sample Product",
            Sku = "SKU-001"
        };

        var reviews = new List<ProductReview>
        {
            new()
            {
                Id = 1,
                ProductId = productId,
                UserId = "user1",
                Rating = 5,
                Comment = "Excellent",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                ProductId = productId,
                UserId = "user2",
                Rating = 3,
                Comment = "Average",
                CreatedAt = DateTime.UtcNow
            }
        };

        var expectedResponse = new List<ProductReviewResponse>
        {
            new(Id: 1, productId, UserId: "user1", Rating: 5, Comment: "Excellent", reviews[0].CreatedAt),
            new(Id: 2, productId, UserId: "user2", Rating: 3, Comment: "Average", reviews[1].CreatedAt)
        };

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewsAsync(productId, query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reviews);
        MapperMock
            .Setup(x => x.Map<List<ProductReviewResponse>>(reviews))
            .Returns(expectedResponse);

        // Act
        var result = await ProductReviewService.GetProductReviewsAsync(productId, query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Success_WhenNoReviewsExist()
    {
        // Arrange
        var productId = 1L;
        var query = new ProductReviewQuery();

        var product = new Product
        {
            Id = productId,
            Name = "Sample Product",
            Sku = "SKU-001"
        };

        var emptyReviews = new List<ProductReview>();
        var emptyResponse = new List<ProductReviewResponse>();

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        ProductReviewRepositoryMock
            .Setup(x => x.GetProductReviewsAsync(productId, query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyReviews);
        MapperMock
            .Setup(x => x.Map<List<ProductReviewResponse>>(emptyReviews))
            .Returns(emptyResponse);

        // Act
        var result = await ProductReviewService.GetProductReviewsAsync(productId, query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(emptyResponse);
    }

    [Fact]
    public async Task Error_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 99L;
        var query = new ProductReviewQuery();

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductReviewService.GetProductReviewsAsync(productId, query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.InvalidRequestError);
        result.ErrorCode.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrowsException()
    {
        // Arrange
        var productId = 1L;
        var query = new ProductReviewQuery();

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Repository failure"));

        // Act
        Func<Task> act = async () => await ProductReviewService.GetProductReviewsAsync(productId, query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Repository failure");
    }
}