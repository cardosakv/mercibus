using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.ProductTests;

public class GetProductsAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string GetProductsUrl = "api/products";

    [Fact]
    public async Task ReturnsOk_WhenProductsExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var category = await dbContext.Categories.AddAsync(new Category { Name = "Electronics" });
        var brand = await dbContext.Brands.AddAsync(new Brand { Name = "BrandX" });
        await dbContext.SaveChangesAsync();

        await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Phone",
                Price = 500,
                Sku = "ELEC001",
                StockQuantity = 50,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });

        await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Laptop",
                Price = 1200,
                Sku = "ELEC002",
                StockQuantity = 30,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });

        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetProductsUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var productList = JsonConvert.DeserializeObject<List<ProductResponse>>(content.Data!.ToString()!);
        productList.Should().NotBeNullOrEmpty();
        productList!.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var allProducts = dbContext.Products.ToList();
        dbContext.Products.RemoveRange(allProducts);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetProductsUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();

        var productList = JsonConvert.DeserializeObject<List<ProductResponse>>(content!.Data!.ToString()!);
        productList.Should().NotBeNull();
        productList.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenInvalidQueryProvided()
    {
        // Arrange
        const int invalidPrice = -100;

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetProductsUrl + "?minPrice=" + invalidPrice);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}