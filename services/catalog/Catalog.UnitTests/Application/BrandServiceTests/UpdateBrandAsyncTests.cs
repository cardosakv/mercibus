using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

public class UpdateBrandAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenBrandIsUpdated()
    {
        // Arrange
        const long brandId = 1;
        var request = new BrandRequest(
            Name: "Updated Brand",
            Description: "Updated Description",
            LogoUrl: "https://example.com/updated-logo.png",
            Region: "Europe",
            Website: "https://updatedbrand.com",
            AdditionalInfo: "Updated Info"
        );

        var existingBrand = new Brand
        {
            Id = brandId,
            Name = "Old Brand",
            Description = "Old Description",
            LogoUrl = "https://example.com/old-logo.png",
            Region = "USA",
            Website = "https://oldbrand.com",
            AdditionalInfo = "Old Info"
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBrand);

        MapperMock
            .Setup(m => m.Map(request, existingBrand));

        BrandRepositoryMock
            .Setup(r => r.UpdateBrandAsync(existingBrand, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await BrandService.UpdateBrandAsync(brandId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsError_WhenBrandNotFound()
    {
        // Arrange
        long brandId = 999;
        var request = new BrandRequest(
            Name: "Some Brand",
            Description: "Some Description"
        );

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Brand?)null);

        // Act
        var result = await BrandService.UpdateBrandAsync(brandId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryUpdateFails()
    {
        // Arrange
        long brandId = 1;
        var request = new BrandRequest(
            Name: "Updated Brand",
            Description: "Updated Desc"
        );

        var existingBrand = new Brand
        {
            Id = brandId,
            Name = "Old",
            Description = "Old Desc"
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBrand);

        MapperMock
            .Setup(m => m.Map(request, existingBrand));

        BrandRepositoryMock
            .Setup(r => r.UpdateBrandAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Update failed"));

        // Act
        Func<Task> act = async () => await BrandService.UpdateBrandAsync(brandId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Update failed");
    }
}