using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

public class GetCategoriesAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenCategoriesAreReturned()
    {
        // Arrange
        var query = new CategoryQuery();

        var categoryList = new List<Category>
        {
            new() { Id = 1, Name = "Electronics", Description = "All electronic items" },
            new() { Id = 2, Name = "Books", Description = "Literature and novels" }
        };

        var mappedList = new List<CategoryResponse>
        {
            new(
                Id: 1,
                ParentCategoryId: null,
                Name: "Electronics",
                Description: "All electronic items"
            ),
            new(
                Id: 2,
                ParentCategoryId: 1,
                Name: "Smartphones",
                Description: "Mobile phones"
            )
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoriesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryList);
        MapperMock
            .Setup(m => m.Map<List<CategoryResponse>>(categoryList))
            .Returns(mappedList);

        // Act
        var result = await CategoryService.GetCategoriesAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Success_WhenNoCategoriesExist()
    {
        // Arrange
        var query = new CategoryQuery();
        var categoryList = new List<Category>();
        var mappedList = new List<CategoryResponse>();

        CategoryRepositoryMock
            .Setup(r => r.GetCategoriesAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryList);
        MapperMock
            .Setup(m => m.Map<List<CategoryResponse>>(categoryList))
            .Returns(mappedList);

        // Act
        var result = await CategoryService.GetCategoriesAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        var query = new CategoryQuery();

        CategoryRepositoryMock
            .Setup(r => r.GetCategoriesAsync(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        // Act
        Func<Task> act = async () => await CategoryService.GetCategoriesAsync(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB error");
    }

    [Fact]
    public async Task Success_WhenCategoryListIsNull()
    {
        // Arrange
        var query = new CategoryQuery();

        CategoryRepositoryMock
            .Setup(r => r.GetCategoriesAsync(query, It.IsAny<CancellationToken>()))!
            .ReturnsAsync((List<Category>?)null);

        MapperMock
            .Setup(m => m.Map<List<CategoryResponse>>(null!))
            .Returns(new List<CategoryResponse>());

        // Act
        var result = await CategoryService.GetCategoriesAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}