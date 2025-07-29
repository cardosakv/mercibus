using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.ProductTests;

public class GetProductByIdAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string GetProductByIdUrl = "api/Products/";
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task ReturnsOk_WhenGetSuccessfully()
    {
        // Arrange 
        var dbContext = factory.CreateDbContext();
        var testCategory = await dbContext.Categories.AddAsync(new Category { Name = "Category 1" });
        var testBrand = await dbContext.Brands.AddAsync(new Brand { Name = "Brand 1" });
        await dbContext.SaveChangesAsync();

        var testProduct = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Test Product",
                Price = 99m,
                Sku = "SKU01",
                StockQuantity = 100,
                CategoryId = testCategory.Entity.Id,
                BrandId = testBrand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _httpClient.GetAsync(GetProductByIdUrl + testProduct.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseProduct = JsonConvert.DeserializeObject<ProductResponse>(content.Data!.ToString()!);
        responseProduct.Should().NotBeNull();
        responseProduct!.Id.Should().Be(testProduct.Entity.Id);
        responseProduct.Name.Should().Be(testProduct.Entity.Name);
        responseProduct.Price.Should().Be(testProduct.Entity.Price);
        responseProduct.Sku.Should().Be(testProduct.Entity.Sku);
        responseProduct.StockQuantity.Should().Be(testProduct.Entity.StockQuantity);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductNotFound()
    {
        // Arrange
        const int testProductId = 999;

        // Act
        var response = await _httpClient.GetAsync(GetProductByIdUrl + testProductId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.ProductNotFound);
    }
}