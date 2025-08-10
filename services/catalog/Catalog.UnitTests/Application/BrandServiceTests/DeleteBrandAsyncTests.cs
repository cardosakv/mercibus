using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

public class DeleteBrandAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenBrandIsDeleted()
    {
        // Arrange
        const long brandId = 1;

        var brand = new Brand
        {
            Id = brandId,
            Name = "To Delete",
            Description = "Brand to be deleted"
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brand);

        BrandRepositoryMock
            .Setup(r => r.IsBrandUsedInProductsAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        BrandRepositoryMock
            .Setup(r => r.DeleteBrandAsync(brand, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await BrandService.DeleteBrandAsync(brandId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsError_WhenBrandNotFound()
    {
        // Arrange
        long brandId = 999;

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Brand?)null);

        // Act
        var result = await BrandService.DeleteBrandAsync(brandId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnsError_WhenBrandIsInUse()
    {
        // Arrange
        long brandId = 10;

        var brand = new Brand
        {
            Id = brandId,
            Name = "Linked Brand"
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brand);

        BrandRepositoryMock
            .Setup(r => r.IsBrandUsedInProductsAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await BrandService.DeleteBrandAsync(brandId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenDeleteFails()
    {
        // Arrange
        long brandId = 5;

        var brand = new Brand
        {
            Id = brandId,
            Name = "Failing Delete"
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brand);

        BrandRepositoryMock
            .Setup(r => r.IsBrandUsedInProductsAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        BrandRepositoryMock
            .Setup(r => r.DeleteBrandAsync(brand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Delete failed"));

        // Act
        Func<Task> act = async () => await BrandService.DeleteBrandAsync(brandId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Delete failed");
    }
}