using System.Net;
using System.Net.Http.Json;
using Catalog.Application.Common;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using Catalog.IntegrationTests.Common;
using FluentAssertions;
using Mercibus.Common.Constants;
using Mercibus.Common.Responses;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.IntegrationTests.ProductTests;

public class DeleteProductImageAsyncTests(BlobWebAppFactory factory) : IClassFixture<BlobWebAppFactory>
{
    private const string DeleteProductImageUrl = "api/products/";

    [Fact]
    public async Task ReturnsOk_WhenProductImageDeletedSuccessfully()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(new Category { Name = "Category for Delete Image" });
        var brand = await dbContext.Brands.AddAsync(new Brand { Name = "Brand for Delete Image" });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Product with Image",
                Description = "Has an image",
                Price = 99,
                Sku = "IMG-DEL-001",
                StockQuantity = 3,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        const string blobName = "delete.png";
        var blobStorageService = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IBlobStorageService>();
        await blobStorageService.UploadFileAsync(blobName, fileStream: new MemoryStream([0xFF, 0xD8, 0xFF, 0xE0]));

        var productImage = await dbContext.ProductImages.AddAsync(
            new ProductImage
            {
                ProductId = product.Entity.Id,
                ImageUrl = blobName,
                AltText = "to delete",
                IsPrimary = true
            });
        await dbContext.SaveChangesAsync();

        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync($"{DeleteProductImageUrl}{product.Entity.Id}/images/{productImage.Entity.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        dbContext = factory.CreateDbContext();
        var deletedImage = await dbContext.ProductImages.FindAsync(productImage.Entity.Id);
        deletedImage.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenProductImageDoesNotExist()
    {
        // Arrange
        var dbContext = factory.CreateDbContext();

        var category = await dbContext.Categories.AddAsync(new Category { Name = "Category for Missing Image" });
        var brand = await dbContext.Brands.AddAsync(new Brand { Name = "Brand for Missing Image" });
        await dbContext.SaveChangesAsync();

        var product = await dbContext.Products.AddAsync(
            new Product
            {
                Name = "Product without Image",
                Description = "Testing missing image delete",
                Price = 49,
                Sku = "IMG-NO-DEL",
                StockQuantity = 7,
                CategoryId = category.Entity.Id,
                BrandId = brand.Entity.Id
            });
        await dbContext.SaveChangesAsync();

        const long nonExistentImageId = 9999;
        var httpClient = factory.CreateClient();

        // Act
        var response = await httpClient.DeleteAsync($"{DeleteProductImageUrl}{product.Entity.Id}/images/{nonExistentImageId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        content.Should().NotBeNull();
        content!.Error.Code.Should().Be(Constants.ErrorCode.ImageNotFound);
        content.Error.Type.Should().Be(ErrorType.InvalidRequestError);
    }
}