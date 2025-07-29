using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Infrastructure;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.ProductTests;

/// <summary>
/// Integration tests for adding a product.
/// </summary>
public class AddProductAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string AddProductUrl = "api/Products";
    private readonly AppDbContext _dbContext = factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task ReturnsOk_WhenAddedSuccessfully()
    {
        // Arrange
        var testCategory = await _dbContext.Categories.AddAsync(new Category { Name = "Category 1" });
        var testBrand = await _dbContext.Brands.AddAsync(new Brand { Name = "Brand 1" });
        await _dbContext.SaveChangesAsync();

        var request = new AddProductRequest(
            Name: "Product 1",
            Description: "A sample product during testing.",
            Sku: "SKU01",
            Price: 99.99m,
            StockQuantity: 100,
            testCategory.Entity.Id,
            testBrand.Entity.Id
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(AddProductUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseProduct = JsonConvert.DeserializeObject<ProductResponse>(content.Data!.ToString()!);
        responseProduct.Should().NotBeNull();
        responseProduct!.Name.Should().Be("Product 1");
        responseProduct.Description.Should().Be("A sample product during testing.");
        responseProduct.Sku.Should().Be("SKU01");
        responseProduct.Price.Should().Be(99.99m);
        responseProduct.StockQuantity.Should().Be(100);

        var savedProduct = await _dbContext.Products.FindAsync(responseProduct.Id);
        savedProduct.Should().NotBeNull();
        savedProduct!.Name.Should().Be("Product 1");
        savedProduct.Description.Should().Be("A sample product during testing.");
        savedProduct.Price.Should().Be(99.99m);
        savedProduct.StockQuantity.Should().Be(100);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new AddProductRequest(
            Name: "", // Invalid name
            Description: "A sample product during testing.",
            Sku: "SKU01",
            Price: 99.99m,
            StockQuantity: 100,
            CategoryId: 1,
            BrandId: 1
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(AddProductUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}