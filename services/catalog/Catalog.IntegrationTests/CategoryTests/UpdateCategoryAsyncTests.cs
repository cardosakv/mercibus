using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace Catalog.IntegrationTests.CategoryTests;

public class UpdateCategoryAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string UpdateCategoryUrl = "api/categories/";

    [Fact]
    public async Task ReturnsOk_WhenCategoryUpdatedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var parent = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Parent Category",
                Description = "Parent"
            });

        var category = await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Old Category",
                Description = "Old description",
                ParentCategoryId = null
            });

        await dbContext.SaveChangesAsync();

        var updateRequest = new CategoryRequest(
            parent.Entity.Id,
            Name: "Updated Category",
            Description: "Updated description"
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateCategoryUrl + category.Entity.Id, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var updatedCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Entity.Id);
        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be(updateRequest.Name);
        updatedCategory.Description.Should().Be(updateRequest.Description);
        updatedCategory.ParentCategoryId.Should().Be(parent.Entity.Id);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenCategoryDoesNotExist()
    {
        // Arrange
        var updateRequest = new CategoryRequest(
            ParentCategoryId: null,
            Name: "Non-existent Category",
            Description: "No match"
        );

        const long nonExistentCategoryId = 9999;

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateCategoryUrl + nonExistentCategoryId, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.CategoryNotFound);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUpdateRequestIsInvalid()
    {
        // Arrange
        var invalidRequest = new CategoryRequest(
            ParentCategoryId: null,
            Name: "", // Invalid
            Description: "Invalid update"
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateCategoryUrl + 1, invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}