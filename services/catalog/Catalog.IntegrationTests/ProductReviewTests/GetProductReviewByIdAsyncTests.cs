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

public class GetProductReviewByIdAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private string GetReviewByIdUrl(long productId, long reviewId)
    {
        return $"api/products/{productId}/reviews/{reviewId}";
    }

    [Fact]
    public async Task ReturnsOk_WhenReviewExists()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Home Appliances"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BrandZ"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Vacuum Cleaner",
            Price = 300,
            Sku = "HOME001",
            StockQuantity = 15,
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
            Comment = "Works well, but a bit noisy."
        };
        await dbContext.ProductReviews.AddAsync(review);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetReviewByIdUrl(product.Id, review.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var reviewResponse = JsonConvert.DeserializeObject<ProductReviewResponse>(content.Data!.ToString()!);
        reviewResponse.Should().NotBeNull();
        reviewResponse!.Id.Should().Be(review.Id);
        reviewResponse.ProductId.Should().Be(product.Id);
        reviewResponse.UserId.Should().Be("user123");
        reviewResponse.Comment.Should().Be("Works well, but a bit noisy.");
    }

    [Fact]
    public async Task ReturnsNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Furniture"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BrandF"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Office Chair",
            Price = 180,
            Sku = "FURN001",
            StockQuantity = 20,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var nonExistentReviewId = 9999L;

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetReviewByIdUrl(product.Id, nonExistentReviewId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.ReviewNotFound);
    }
}