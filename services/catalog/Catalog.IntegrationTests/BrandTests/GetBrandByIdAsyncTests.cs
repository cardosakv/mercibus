using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.BrandTests;

/// <summary>
/// Integration tests for retrieving a brand by ID.
/// </summary>
public class GetBrandByIdAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string GetBrandByIdUrl = "api/brands/";

    [Fact]
    public async Task ReturnsOk_WhenBrandRetrievedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Apple",
                Description = "Premium electronics brand"
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.GetAsync(GetBrandByIdUrl + brand.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseBrand = JsonConvert.DeserializeObject<BrandResponse>(content.Data!.ToString()!);
        responseBrand.Should().NotBeNull();
        responseBrand!.Id.Should().Be(brand.Entity.Id);
        responseBrand.Name.Should().Be(brand.Entity.Name);
        responseBrand.Description.Should().Be(brand.Entity.Description);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenBrandNotFound()
    {
        // Arrange
        const long nonExistentBrandId = 9999;

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.GetAsync(GetBrandByIdUrl + nonExistentBrandId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.BrandNotFound);
    }
}