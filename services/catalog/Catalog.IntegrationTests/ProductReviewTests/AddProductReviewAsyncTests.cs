using System.Net;
using System.Net.Http.Headers;
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

/// <summary>
///     Integration tests for adding a product review.
/// </summary>
public class AddProductReviewAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private static string GetAddReviewUrl(long productId)
    {
        return $"api/Products/{productId}/Reviews";
    }

    [Fact]
    public async Task ReturnsOk_WhenReviewIsAddedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        // Add a product for review
        var testCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Category 1"
            });
        var testBrand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand 1"
            });
        await dbContext.SaveChangesAsync();

        var testProduct = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Test Product",
                Description = "Integration test product",
                Sku = "SKU-TEST-01",
                Price = 49.99m,
                StockQuantity = 50,
                CategoryId = testCategory.Entity.Id,
                BrandId = testBrand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        var request = new ProductReviewRequest(
            Rating: 5,
            Comment: "Excellent product!"
        );

        var httpClient = factory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: "test-token");

        // Act
        var response = await httpClient.PostAsJsonAsync(requestUri: GetAddReviewUrl(testProduct.Entity.Id), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseReview = JsonConvert.DeserializeObject<ProductReviewResponse>(content.Data!.ToString()!);
        responseReview.Should().NotBeNull();
        responseReview!.Rating.Should().Be(5);
        responseReview.Comment.Should().Be("Excellent product!");
        responseReview.ProductId.Should().Be(testProduct.Entity.Id);

        dbContext = factory.CreateDbContext();
        var savedReview = await dbContext.ProductReviews.FindAsync(responseReview.Id);
        savedReview.Should().NotBeNull();
        savedReview!.Rating.Should().Be(5);
        savedReview.Comment.Should().Be("Excellent product!");
        savedReview.ProductId.Should().Be(testProduct.Entity.Id);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductDoesNotExist()
    {
        // Arrange
        var request = new ProductReviewRequest(
            Rating: 3,
            Comment: "Non-existent product test"
        );

        var httpClient = factory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: "test-token");

        // Act
        var response = await httpClient.PostAsJsonAsync(requestUri: GetAddReviewUrl(productId: 9999), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.ProductNotFound);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var testCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Category 1"
            });
        var testBrand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand 1"
            });
        await dbContext.SaveChangesAsync();

        var testProduct = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Test Product",
                Description = "Integration test product",
                Sku = "SKU-TEST-03",
                Price = 19.99m,
                StockQuantity = 10,
                CategoryId = testCategory.Entity.Id,
                BrandId = testBrand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        var request = new ProductReviewRequest(
            Rating: 0, // invalid rating
            Comment: ""
        );

        var httpClient = factory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: "test-token");

        // Act
        var response = await httpClient.PostAsJsonAsync(requestUri: GetAddReviewUrl(testProduct.Entity.Id), request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}