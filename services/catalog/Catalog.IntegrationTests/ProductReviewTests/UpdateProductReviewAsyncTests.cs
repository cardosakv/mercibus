using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.ProductReviewTests;

public class UpdateProductReviewAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private string UpdateReviewUrl(long productId, long reviewId)
    {
        return $"api/products/{productId}/reviews/{reviewId}";
    }

    [Fact]
    public async Task ReturnsOk_WhenReviewIsUpdatedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Books"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BookBrand"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Novel",
            Price = 15,
            Sku = "BOOK001",
            StockQuantity = 200,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var review = new ProductReview
        {
            ProductId = product.Id,
            UserId = "user123",
            Rating = 3,
            Comment = "It was okay."
        };
        await dbContext.ProductReviews.AddAsync(review);
        await dbContext.SaveChangesAsync();

        var request = new ProductReviewRequest(
            Rating: 5,
            Comment: "Loved it!"
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateReviewUrl(product.Id, review.Id), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var updatedReview = JsonConvert.DeserializeObject<ProductReviewResponse>(content.Data!.ToString()!);
        updatedReview.Should().NotBeNull();
        updatedReview!.Id.Should().Be(review.Id);
        updatedReview.ProductId.Should().Be(product.Id);
        updatedReview.UserId.Should().Be("user123");
        updatedReview.Rating.Should().Be(5);
        updatedReview.Comment.Should().Be("Loved it!");

        // Verify in DB
        dbContext = factory.CreateDbContext();
        var savedReview = await dbContext.ProductReviews.FindAsync(review.Id);
        savedReview.Should().NotBeNull();
        savedReview!.Rating.Should().Be(5);
        savedReview.Comment.Should().Be("Loved it!");
    }

    [Fact]
    public async Task ReturnsNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Toys"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "ToyBrand"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Action Figure",
            Price = 25,
            Sku = "TOY001",
            StockQuantity = 100,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var nonExistentReviewId = 9999L;
        var request = new ProductReviewRequest(
            Rating: 2,
            Comment: "Not great."
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateReviewUrl(product.Id, nonExistentReviewId), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }
}