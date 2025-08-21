using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.BrandTests;

/// <summary>
/// Integration tests for retrieving brands.
/// </summary>
public class GetBrandsAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string GetBrandsUrl = "api/brands";

    [Fact]
    public async Task ReturnsOk_WhenBrandsExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BrandX",
                Description = "Electronics brand"
            });

        await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "BrandY",
                Description = "Clothing brand"
            });

        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetBrandsUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var brandList = JsonConvert.DeserializeObject<List<BrandResponse>>(content.Data!.ToString()!);
        brandList.Should().NotBeNullOrEmpty();
        brandList!.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task ReturnsEmptyList_WhenNoBrandsExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var allBrands = dbContext.Brands.ToList();
        dbContext.Brands.RemoveRange(allBrands);
        await dbContext.SaveChangesAsync();

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.GetAsync(GetBrandsUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();

        var brandList = JsonConvert.DeserializeObject<List<BrandResponse>>(content!.Data!.ToString()!);
        brandList.Should().NotBeNull();
        brandList.Should().BeEmpty();
    }
}