using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

public class GetBrandByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenBrandIsFound()
    {
        // Arrange
        long brandId = 200;

        var brand = new Brand
        {
            Id = brandId,
            Name = "BrandX",
            Description = "Leading electronics brand",
            LogoUrl = "https://example.com/logo.png",
            Region = "USA",
            Website = "https://brandx.com",
            AdditionalInfo = "Founded in 1990"
        };

        var mappedResponse = new BrandResponse(
            brandId,
            Name: "BrandX",
            Description: "Leading electronics brand",
            LogoUrl: "https://example.com/logo.png",
            Region: "USA",
            Website: "https://brandx.com",
            AdditionalInfo: "Founded in 1990"
        );

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(brand);

        MapperMock
            .Setup(m => m.Map<BrandResponse>(brand))
            .Returns(mappedResponse);

        // Act
        var result = await BrandService.GetBrandByIdAsync(brandId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeOfType<BrandResponse>();

        var data = result.Data.Should().BeOfType<BrandResponse>().Subject;
        data.Id.Should().Be(brandId);
        data.Name.Should().Be("BrandX");
        data.Description.Should().Be("Leading electronics brand");
        data.LogoUrl.Should().Be("https://example.com/logo.png");
        data.Region.Should().Be("USA");
        data.Website.Should().Be("https://brandx.com");
        data.AdditionalInfo.Should().Be("Founded in 1990");
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
        var result = await BrandService.GetBrandByIdAsync(brandId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        long brandId = 1;

        BrandRepositoryMock
            .Setup(r => r.GetBrandByIdAsync(brandId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        Func<Task> act = async () => await BrandService.GetBrandByIdAsync(brandId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database connection failed");
    }
}