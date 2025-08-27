using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.ProductReviewTests;

public class GetProductReviewsAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private string GetReviewsUrl(long productId)
    {
        return $"api/products/{productId}/reviews";
    }

    [Fact]
    public async Task ReturnsOk_WhenReviewsExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Electronics"
            });
        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BrandX"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "Headphones",
            Price = 150,
            Sku = "ELEC100",
            StockQuantity = 25,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        var review1 = new ProductReview
        {
            ProductId = product.Id,
            UserId = "user1",
            Rating = 5,
            Comment = "Great sound quality!"
        };
        var review2 = new ProductReview
        {
            ProductId = product.Id,
            UserId = "user2",
            Rating = 3,
            Comment = "Average build."
        };

        await dbContext.ProductReviews.AddRangeAsync(review1, review2);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetReviewsUrl(product.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var reviews = JsonConvert.DeserializeObject<List<ProductReviewResponse>>(content.Data!.ToString()!);
        reviews.Should().NotBeNullOrEmpty();
        reviews!.Count.Should().Be(2);
        reviews[1].Comment.Should().Be("Great sound quality!");
        reviews[0].Comment.Should().Be("Average build.");
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenNoReviewsExist()
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
                Name = "BrandY"
            });
        await dbContext.SaveChangesAsync();

        var product = new Product
        {
            Name = "E-Reader",
            Price = 200,
            Sku = "BOOK001",
            StockQuantity = 40,
            CategoryId = category.Entity.Id,
            BrandId = brand.Entity.Id
        };
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetReviewsUrl(product.Id));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();

        var reviews = JsonConvert.DeserializeObject<List<ProductReviewResponse>>(content!.Data!.ToString()!);
        reviews.Should().NotBeNull();
        reviews.Should().BeEmpty();
    }
}