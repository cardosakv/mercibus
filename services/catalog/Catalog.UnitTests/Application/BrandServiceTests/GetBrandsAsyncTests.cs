using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

public class GetBrandsAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenBrandsAreReturned()
    {
        // Arrange
        var query = new BrandQuery();

        var brandList = new List<Brand>
        {
            new() { Id = 1, Name = "BrandX", Description = "Electronics manufacturer" },
            new() { Id = 2, Name = "BrandY", Description = "Book publisher" }
        };

        var mappedList = new List<BrandResponse>
        {
            new(
                Id: 1,
                Name: "BrandX",
                Description: "Electronics manufacturer",
                LogoUrl: "https://example.com/logo1.png",
                Region: "USA",
                Website: "https://brandx.com",
                AdditionalInfo: "Top seller"
            ),
            new(
                Id: 2,
                Name: "BrandY",
                Description: "Book publisher",
                LogoUrl: null,
                Region: "UK",
                Website: null,
                AdditionalInfo: null
            )
        };

        BrandRepositoryMock
            .Setup(r => r.GetBrandsAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brandList);

        MapperMock
            .Setup(m => m.Map<List<BrandResponse>>(brandList))
            .Returns(mappedList);

        // Act
        var result = await BrandService.GetBrandsAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Success_WhenNoBrandsExist()
    {
        // Arrange
        var query = new BrandQuery();
        var brandList = new List<Brand>();
        var mappedList = new List<BrandResponse>();

        BrandRepositoryMock
            .Setup(r => r.GetBrandsAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brandList);

        MapperMock
            .Setup(m => m.Map<List<BrandResponse>>(brandList))
            .Returns(mappedList);

        // Act
        var result = await BrandService.GetBrandsAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        var query = new BrandQuery();

        BrandRepositoryMock
            .Setup(r => r.GetBrandsAsync(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        // Act
        Func<Task> act = async () => await BrandService.GetBrandsAsync(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB error");
    }

    [Fact]
    public async Task Success_WhenBrandListIsNull()
    {
        // Arrange
        var query = new BrandQuery();

        BrandRepositoryMock
            .Setup(r => r.GetBrandsAsync(query, It.IsAny<CancellationToken>()))!
            .ReturnsAsync((List<Brand>?)null);

        MapperMock
            .Setup(m => m.Map<List<BrandResponse>>(null!))
            .Returns(new List<BrandResponse>());

        // Act
        var result = await BrandService.GetBrandsAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}