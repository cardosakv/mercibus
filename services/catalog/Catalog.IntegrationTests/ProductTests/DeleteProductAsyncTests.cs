using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;

namespace Catalog.IntegrationTests.ProductTests;

public class DeleteProductAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string DeleteProductUrl = "api/products/";
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task ReturnsOk_WhenProductDeletedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var category = await dbContext.Categories.AddAsync(new Category { Name = "Category for Delete" });
        var brand = await dbContext.Brands.AddAsync(new Brand { Name = "Brand for Delete" });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "To Be Deleted",
                Description = "Product for deletion test",
                Price = 10.0m,
                Sku = "DELETE-ME",
                StockQuantity = 5,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _httpClient.DeleteAsync(DeleteProductUrl + product.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var deleted = await dbContext.Products.FindAsync(product.Entity.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductDoesNotExist()
    {
        // Arrange
        const long nonExistentProductId = 9999;

        // Act
        var response = await _httpClient.DeleteAsync(DeleteProductUrl + nonExistentProductId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.ProductNotFound);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}