using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

public class AddBrandAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenBrandIsAdded_WithAllFields()
    {
        // Arrange
        var request = new BrandRequest(
            Name: "Acme Corp",
            Description: "Leading supplier of gadgets",
            LogoUrl: "http://example.com/logo.png",
            Region: "Global",
            Website: "http://acmecorp.com",
            AdditionalInfo: "Founded in 1980"
        );

        var entity = new Brand
        {
            Name = request.Name,
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            Region = request.Region,
            Website = request.Website,
            AdditionalInfo = request.AdditionalInfo,
            CreatedAt = DateTime.UtcNow
        };

        var savedBrand = entity;
        savedBrand.Id = 100;

        MapperMock.Setup(m => m.Map<Brand>(request)).Returns(entity);
        BrandRepositoryMock.Setup(r => r.AddBrandAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedBrand);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock.Setup(m => m.Map<BrandResponse>(savedBrand))
            .Returns(
                new BrandResponse(
                    Id: 100,
                    savedBrand.Name,
                    savedBrand.Description,
                    savedBrand.LogoUrl,
                    savedBrand.Region,
                    savedBrand.Website,
                    savedBrand.AdditionalInfo
                )
            );

        // Act
        var result = await BrandService.AddBrandAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Data.Should().BeOfType<BrandResponse>().Subject;
        data.Id.Should().Be(100);
        data.Name.Should().Be(request.Name);
        data.Description.Should().Be(request.Description);
        data.LogoUrl.Should().Be(request.LogoUrl);
        data.Region.Should().Be(request.Region);
        data.Website.Should().Be(request.Website);
        data.AdditionalInfo.Should().Be(request.AdditionalInfo);
    }

    [Fact]
    public async Task Success_WhenBrandIsAdded_WithMinimalFields()
    {
        // Arrange
        var request = new BrandRequest(
            Name: "Minimal Brand"
        );

        var entity = new Brand
        {
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        var savedBrand = entity;
        savedBrand.Id = 200;

        MapperMock.Setup(m => m.Map<Brand>(request)).Returns(entity);
        BrandRepositoryMock.Setup(r => r.AddBrandAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedBrand);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock.Setup(m => m.Map<BrandResponse>(savedBrand))
            .Returns(
                new BrandResponse(
                    Id: 200,
                    savedBrand.Name
                )
            );

        // Act
        var result = await BrandService.AddBrandAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Data.Should().BeOfType<BrandResponse>().Subject;
        data.Id.Should().Be(200);
        data.Name.Should().Be(request.Name);
        data.Description.Should().BeNull();
        data.LogoUrl.Should().BeNull();
        data.Region.Should().BeNull();
        data.Website.Should().BeNull();
        data.AdditionalInfo.Should().BeNull();
    }

    [Fact]
    public async Task Throws_WhenAddFails()
    {
        // Arrange
        var request = new BrandRequest(
            Name: "Faulty Brand",
            Description: "Should fail"
        );

        var entity = new Brand
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        MapperMock.Setup(m => m.Map<Brand>(request)).Returns(entity);
        BrandRepositoryMock
            .Setup(r => r.AddBrandAsync(It.IsAny<Brand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Insert failed"));

        // Act
        Func<Task> act = async () => await BrandService.AddBrandAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insert failed");
    }
}