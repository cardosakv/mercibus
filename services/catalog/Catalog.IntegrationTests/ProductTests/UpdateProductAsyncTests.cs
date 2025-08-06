using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Catalog.IntegrationTests.ProductTests;

public class UpdateProductAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string UpdateProductUrl = "api/products/";

    [Fact]
    public async Task ReturnsOk_WhenUpdatedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var category = await dbContext.Categories.AddAsync(new Category { Name = "Books" });
        var brand = await dbContext.Brands.AddAsync(new Brand { Name = "BrandA" });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Old Product",
                Description = "Old description",
                Price = 50,
                Sku = "SKU-OLD",
                StockQuantity = 10,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        var updateRequest = new ProductRequest(
            Name: "Updated Product",
            Description: "Updated description",
            Sku: "SKU-NEW",
            Price: 75,
            StockQuantity: 20,
            category.Entity.Id,
            brand.Entity.Id
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateProductUrl + product.Entity.Id, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var updatedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Entity.Id);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be(updateRequest.Name);
        updatedProduct.Description.Should().Be(updateRequest.Description);
        updatedProduct.Sku.Should().Be(updateRequest.Sku);
        updatedProduct.Price.Should().Be(updateRequest.Price);
        updatedProduct.StockQuantity.Should().Be(updateRequest.StockQuantity);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductDoesNotExist()
    {
        // Arrange
        var updateRequest = new ProductRequest(
            Name: "Non-existent Product",
            Description: "Does not exist",
            Sku: "SKU404",
            Price: 10,
            StockQuantity: 0,
            CategoryId: 1,
            BrandId: 1
        );

        const long nonExistentProductId = 9999;

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateProductUrl + nonExistentProductId, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUpdateRequestIsInvalid()
    {
        // Arrange
        var invalidRequest = new ProductRequest(
            Name: "", // Invalid
            Description: "Desc",
            Sku: "", // Invalid
            Price: -99.99m, // Invalid
            StockQuantity: -5, // Invalid
            CategoryId: 0, // Invalid
            BrandId: 0 // Invalid
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateProductUrl + 999, invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}