using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

public class AddProductReviewAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenReviewIsAdded()
    {
        // Arrange
        var productId = 1L;
        var userId = "user1";
        var request = new ProductReviewRequest(Rating: 5, Comment: "Amazing product!");

        var product = new Product
        {
            Id = productId,
            Name = "Sample Product",
            Sku = "SKU-001"
        };

        var entity = new ProductReview
        {
            Id = 1,
            ProductId = productId,
            UserId = userId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        var response = new ProductReviewResponse(
            entity.Id,
            entity.ProductId,
            entity.UserId,
            entity.Rating,
            entity.Comment,
            entity.CreatedAt
        );

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        MapperMock
            .Setup(x => x.Map<ProductReview>(request))
            .Returns(entity);
        ProductReviewRepositoryMock
            .Setup(x => x.AddProductReviewAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);
        DbContextMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock
            .Setup(x => x.Map<ProductReviewResponse>(entity))
            .Returns(response);

        // Act
        var result = await ProductReviewService.AddProductReviewAsync(productId, userId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Error_WhenUserIdIsNull()
    {
        // Arrange
        var productId = 1L;
        string? userId = null;
        var request = new ProductReviewRequest(Rating: 4, Comment: "Decent product");

        // Act
        var result = await ProductReviewService.AddProductReviewAsync(productId, userId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ErrorType.AuthenticationError);
        result.ErrorCode.Should().Be(ErrorCode.Unauthorized);
    }

    [Fact]
    public async Task Error_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 99L;
        var userId = "user1";
        var request = new ProductReviewRequest(Rating: 3, Comment: "Not great");

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await ProductReviewService.AddProductReviewAsync(productId, userId, request, CancellationToken.None);

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
        var userId = "user1";
        var request = new ProductReviewRequest(Rating: 5, Comment: "Test review");

        var product = new Product
        {
            Id = productId,
            Name = "Sample Product",
            Sku = "SKU-001"
        };

        ProductRepositoryMock
            .Setup(x => x.GetProductByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        MapperMock
            .Setup(x => x.Map<ProductReview>(request))
            .Throws(new InvalidOperationException("Mapping failed"));

        // Act
        Func<Task> act = async () => await ProductReviewService.AddProductReviewAsync(productId, userId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Mapping failed");
    }
}