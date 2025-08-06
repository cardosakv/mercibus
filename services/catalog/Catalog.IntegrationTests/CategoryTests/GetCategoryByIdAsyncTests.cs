using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.CategoryTests;

/// <summary>
/// Integration tests for retrieving a category by ID.
/// </summary>
public class GetCategoryByIdAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string GetCategoryByIdUrl = "api/categories/";

    [Fact]
    public async Task ReturnsOk_WhenCategoryRetrievedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var parentCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Parent Category",
                Description = "Parent description"
            });

        await dbContext.SaveChangesAsync();

        var childCategory = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Child Category",
                Description = "Child description",
                ParentCategoryId = parentCategory.Entity.Id
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.GetAsync(GetCategoryByIdUrl + childCategory.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseCategory = JsonConvert.DeserializeObject<CategoryResponse>(content.Data!.ToString()!);
        responseCategory.Should().NotBeNull();
        responseCategory!.Id.Should().Be(childCategory.Entity.Id);
        responseCategory.Name.Should().Be(childCategory.Entity.Name);
        responseCategory.Description.Should().Be(childCategory.Entity.Description);
        responseCategory.ParentCategoryId.Should().Be(parentCategory.Entity.Id);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenCategoryNotFound()
    {
        // Arrange
        const long nonExistentCategoryId = 9999;

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.GetAsync(GetCategoryByIdUrl + nonExistentCategoryId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.CategoryNotFound);
    }
}