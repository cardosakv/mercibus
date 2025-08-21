using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;

namespace Catalog.IntegrationTests.CategoryTests;

/// <summary>
/// Integration tests for deleting a category.
/// </summary>
public class DeleteCategoryAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string DeleteCategoryUrl = "api/categories/";

    [Fact]
    public async Task ReturnsOk_WhenCategoryDeletedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Category To Delete",
                Description = "Test deletion",
                ParentCategoryId = null
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteCategoryUrl + category.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var deleted = await dbContext.Categories.FindAsync(category.Entity.Id);
        deleted.Should().BeNull(); // Confirm deletion
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenCategoryDoesNotExist()
    {
        // Arrange
        const long nonExistentCategoryId = 9999;

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteCategoryUrl + nonExistentCategoryId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.CategoryNotFound);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenCategoryIsUsedInProducts()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Used Category"
            });

        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Brand for Category Use"
            });

        await dbContext.SaveChangesAsync();

        await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Product with Used Category",
                Sku = "USED-123",
                Price = 10,
                StockQuantity = 1,
                Description = "Dummy product",
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync(DeleteCategoryUrl + category.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.CategoryInUse);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}