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

namespace Catalog.IntegrationTests.BrandTests;

public class UpdateBrandAsyncTests(DbWebAppFactory factory) : IClassFixture<DbWebAppFactory>
{
    private const string UpdateBrandUrl = "api/brands/";

    [Fact]
    public async Task ReturnsOk_WhenBrandUpdatedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var brand = await dbContext.Brands.AddAsync(
            new Brand
            {
                Name = "Old Brand",
                Description = "Old description"
            });

        await dbContext.SaveChangesAsync();

        var updateRequest = new BrandRequest(
            Name: "Updated Brand",
            Description: "Updated description"
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateBrandUrl + brand.Entity.Id, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var updatedBrand = await dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brand.Entity.Id);
        updatedBrand.Should().NotBeNull();
        updatedBrand!.Name.Should().Be(updateRequest.Name);
        updatedBrand.Description.Should().Be(updateRequest.Description);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenBrandDoesNotExist()
    {
        // Arrange
        var updateRequest = new BrandRequest(
            Name: "Non-existent Brand",
            Description: "No match"
        );

        const long nonExistentBrandId = 9999;

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateBrandUrl + nonExistentBrandId, updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        content.Error.Code.Should().Be(Constants.ErrorCode.BrandNotFound);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenUpdateRequestIsInvalid()
    {
        // Arrange
        var invalidRequest = new BrandRequest(
            Name: "", // Invalid
            Description: "Invalid update"
        );

        // Act
        var httpClient = factory.CreateClient();
        var response = await httpClient.PutAsJsonAsync(requestUri: UpdateBrandUrl + 1, invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}