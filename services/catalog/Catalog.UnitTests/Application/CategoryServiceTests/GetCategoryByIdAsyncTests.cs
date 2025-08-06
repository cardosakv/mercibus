using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

public class GetCategoryByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenCategoryIsFound()
    {
        // Arrange
        long categoryId = 100;

        var category = new Category
        {
            Id = categoryId,
            Name = "Home Appliances",
            Description = "Appliances for home use",
            ParentCategoryId = null
        };

        var mappedResponse = new CategoryResponse(
            Id: 100,
            ParentCategoryId: null,
            Name: "Home Appliances",
            Description: "Appliances for home use"
        );

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        MapperMock
            .Setup(m => m.Map<CategoryResponse>(category))
            .Returns(mappedResponse);

        // Act
        var result = await CategoryService.GetCategoryByIdAsync(categoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeOfType<CategoryResponse>();

        var data = result.Data.Should().BeOfType<CategoryResponse>().Subject;
        data.Id.Should().Be(categoryId);
        data.Name.Should().Be("Home Appliances");
        data.Description.Should().Be("Appliances for home use");
        data.ParentCategoryId.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsError_WhenCategoryNotFound()
    {
        // Arrange
        long categoryId = 999;

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await CategoryService.GetCategoryByIdAsync(categoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        long categoryId = 1;

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        Func<Task> act = async () => await CategoryService.GetCategoryByIdAsync(categoryId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database connection failed");
    }
}