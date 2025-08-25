using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;

namespace Catalog.IntegrationTests.ProductReviewTests;

public class DeleteProductReviewAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private string DeleteReviewUrl(long productId, long reviewId)
    {
        return $"api/products/{productId}/reviews/{reviewId}";
    }

    [Fact]
    public async Task ReturnsOk_WhenReviewIsDeletedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Games"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "GameBrand"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Board Game",
            Price = 35,
            Sku = "GAME001",
            StockQuantity = 20,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var review = new ProductReview
        {
            ProductId = product.Id,
            UserId = "user123",
            Rating = 4,
            Comment = "Fun to play!"
        };
        await dbContext.ProductReviews.AddAsync(review);
        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteReviewUrl(product.Id, review.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();

        dbContext = factory.CreateDbContext();
        var deletedReview = await dbContext.ProductReviews.FindAsync(review.Id);
        deletedReview.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Music"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "MusicBrand"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Guitar",
            Price = 150,
            Sku = "MUS001",
            StockQuantity = 10,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var nonExistentReviewId = 9999L;

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteReviewUrl(product.Id, nonExistentReviewId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }
}