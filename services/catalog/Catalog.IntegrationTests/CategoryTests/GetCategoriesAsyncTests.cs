using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.CategoryTests;

/// <summary>
/// Integration tests for retrieving categories.
/// </summary>
public class GetCategoriesAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string GetCategoriesUrl = "api/categories";

    [Fact]
    public async Task ReturnsOk_WhenCategoriesExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Electronics",
                Description = "Devices and gadgets"
            });

        await dbContext.Categories.AddAsync(
            new Category
            {
                Name = "Books",
                Description = "Printed media"
            });

        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetCategoriesUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var categoryList = JsonConvert.DeserializeObject<List<CategoryResponse>>(content.Data!.ToString()!);
        categoryList.Should().NotBeNullOrEmpty();
        categoryList!.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenNoCategoriesExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var allCategories = dbContext.Categories.ToList();
        dbContext.Categories.RemoveRange(allCategories);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetCategoriesUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();

        var categoryList = JsonConvert.DeserializeObject<List<CategoryResponse>>(content!.Data!.ToString()!);
        categoryList.Should().NotBeNull();
        categoryList.Should().BeEmpty();
    }
}