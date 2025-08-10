using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;

namespace Catalog.IntegrationTests.BrandTests;

/// <summary>
/// Integration tests for deleting a brand.
/// </summary>
public class DeleteBrandAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string DeleteBrandUrl = "api/brands/";

    [Fact]
    public async Task ReturnsOk_WhenBrandDeletedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand To Delete",
                Description = "Test brand deletion"
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteBrandUrl + brand.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var deleted = await dbContext.Brands.FindAsync(brand.Entity.Id);
        deleted.Should().BeNull(); // Confirm deletion
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenBrandDoesNotExist()
    {
        // Arrange
        const long nonExistentBrandId = 9999;

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteBrandUrl + nonExistentBrandId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.BrandNotFound);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenBrandIsUsedInProducts()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Used Brand"
            });

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Category for Brand Use"
            });

        await dbContext.SaveChangesAsync();

        await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Product with Used Brand",
                Sku = "USED-BRAND-123",
                Price = 20,
                StockQuantity = 5,
                Description = "Product linked to brand",
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteBrandUrl + brand.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.BrandInUse);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}