using System.Net;
using System.Net.Http.Json;
using Catalog.Application.DTOs;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Catalog.IntegrationTests.BrandTests;

/// <summary>
/// Integration tests for adding a brand.
/// </summary>
public class AddBrandAsyncTests(TestWebAppFactory factory) : IClassFixture<TestWebAppFactory>
{
    private const string AddBrandUrl = "api/Brands";

    [Fact]
    public async Task ReturnsOk_WhenBrandIsAddedSuccessfully()
    {
        // Arrange
        var request = new BrandRequest(
            Name: "Test Brand",
            Description: "A new brand for testing",
            LogoUrl: "https://example.com/logo.png",
            Region: "North America",
            Website: "https://testbrand.com",
            AdditionalInfo: "Some additional info"
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync(AddBrandUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        content.Should().NotBeNull();
        content!.Data.Should().NotBeNull();

        var responseBrand = JsonConvert.DeserializeObject<BrandResponse>(content.Data!.ToString()!);
        responseBrand.Should().NotBeNull();
        responseBrand!.Name.Should().Be("Test Brand");
        responseBrand.Description.Should().Be("A new brand for testing");
        responseBrand.LogoUrl.Should().Be("https://example.com/logo.png");
        responseBrand.Region.Should().Be("North America");
        responseBrand.Website.Should().Be("https://testbrand.com");
        responseBrand.AdditionalInfo.Should().Be("Some additional info");

        var dbContext = factory.CreateDbContext();
        var savedBrand = await dbContext.Brands.FindAsync(responseBrand.Id);
        savedBrand.Should().NotBeNull();
        savedBrand!.Name.Should().Be("Test Brand");
        savedBrand.Description.Should().Be("A new brand for testing");
        savedBrand.LogoUrl.Should().Be("https://example.com/logo.png");
        savedBrand.Region.Should().Be("North America");
        savedBrand.Website.Should().Be("https://testbrand.com");
        savedBrand.AdditionalInfo.Should().Be("Some additional info");
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new BrandRequest(
            Name: "", // Invalid: empty name
            Description: "Invalid brand with missing fields"
        );

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.PostAsJsonAsync(AddBrandUrl, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Should().NotBeNull();
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}