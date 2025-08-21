using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.CategoryTests;

/// <summary>
/// Integration tests for adding a category.
/// </summary>
public class AddCategoryAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string AddCategoryUrl = "api/Categories";

    [Fact]
    public async Task ReturnsOk_WhenCategoryIsAddedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var parentCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Parent Category",
                Description = "A parent category"
            });
        await dbContext.SaveChangesAsync();

        var request = new CategoryRequest(
            parentCategory.Entity.Id,
            Name: "Subcategory",
            Description: "A new subcategory"
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync(AddCategoryUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseCategory = JsonConvert.DeserializeObject<CategoryResponse>(content.Data!.ToString()!);
        responseCategory.Should().NotBeNull();
        responseCategory!.Name.Should().Be("Subcategory");
        responseCategory.Description.Should().Be("A new subcategory");
        responseCategory.ParentCategoryId.Should().Be(parentCategory.Entity.Id);

        dbContext = factory.CreateDbContext();
        var savedCategory = await dbContext.Categories.FindAsync(responseCategory.Id);
        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be("Subcategory");
        savedCategory.Description.Should().Be("A new subcategory");
        savedCategory.ParentCategoryId.Should().Be(parentCategory.Entity.Id);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new CategoryRequest(
            ParentCategoryId: null,
            Name: "", // Invalid (empty name)
            Description: "Testing invalid input"
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync(AddCategoryUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}