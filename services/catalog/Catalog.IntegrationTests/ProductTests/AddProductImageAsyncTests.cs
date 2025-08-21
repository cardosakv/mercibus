using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Catalog.Application.Common;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;

namespace Catalog.IntegrationTests.ProductTests;

/// <summary>
/// Integration tests for adding a product image.
/// </summary>
public class AddProductImageAsyncTests(BlobWebAppFactory factory) : IClassFixture<BlobWebAppFactory>
{
    private const string AddProductImageUrl = "api/products/";

    [Fact]
    public async Task ReturnsOk_WhenProductImageIsAddedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();
        var testCategory = await dbContext.Categories.AddAsync(new Category { Name = "Category 1" });
        var testBrand = await dbContext.Brands.AddAsync(new Brand { Name = "Brand 1" });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Camera",
                Sku = "CAM-123",
                Price = 499,
                StockQuantity = 10,
                Description = "Test product",
                CategoryId = testCategory.Entity.Id,
                BrandId = testBrand.Entity.Id
            });

        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Build multipart form data for file upload
        var content = new MultipartFormDataContent();
        var imageBytes = new byte[]
        {
            0xFF, 0xD8, // JPEG SOI marker
            0xFF, 0xE0, 0x00, 0x10, // APP0 marker
            0x4A, 0x46, 0x49, 0x46, // 'JFIF'
            0x00, // '\0'
            0xFF, 0xD9 // JPEG EOI marker
        };
        var byteArrayContent = new ByteArrayContent(imageBytes);
        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        content.Add(byteArrayContent, name: "Image", fileName: "test.png");
        content.Add(content: new StringContent("true"), name: "IsPrimary");
        content.Add(content: new StringContent("Alt text for image"), name: "AltText");

        // Act
        var response = await httpClient.PostAsync(requestUri: $"{AddProductImageUrl}{product.Entity.Id}/images", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductDoesNotExist()
    {
        // Arrange
        var httpClient = factory.CreateClient();

        var content = new MultipartFormDataContent();
        var imageBytes = Encoding.UTF8.GetBytes("fake-image-content");
        var byteArrayContent = new ByteArrayContent(imageBytes);
        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        content.Add(byteArrayContent, name: "Image", fileName: "test.png");
        content.Add(content: new StringContent("true"), name: "IsPrimary");
        content.Add(content: new StringContent("Alt text for image"), name: "AltText");

        const long nonExistentProductId = 9999;

        // Act
        var response = await httpClient.PostAsync(requestUri: $"{AddProductImageUrl}{nonExistentProductId}/images", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Error.Should().NotBeNull();
        apiResponse.Error.Type.Should().Be(ErrorType.InvalidRequestError);
        apiResponse.Error.Code.Should().Be(Constants.ErrorCode.ProductNotFound);
    }
}