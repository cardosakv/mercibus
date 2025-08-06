using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

public class UpdateCategoryAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenCategoryIsUpdated()
    {
        // Arrange
        const long categoryId = 1;
        var request = new CategoryRequest(
            ParentCategoryId: null,
            Name: "Updated Name",
            Description: "Updated Description"
        );

        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Old Name",
            Description = "Old Description",
            ParentCategoryId = null
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        MapperMock
            .Setup(m => m.Map(request, existingCategory));

        CategoryRepositoryMock
            .Setup(r => r.UpdateCategoryAsync(existingCategory, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await CategoryService.UpdateCategoryAsync(categoryId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ReturnsError_WhenCategoryNotFound()
    {
        // Arrange
        long categoryId = 999;
        var request = new CategoryRequest(ParentCategoryId: null, Name: "Some Name", Description: "Some Desc");

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await CategoryService.UpdateCategoryAsync(categoryId, request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryUpdateFails()
    {
        // Arrange
        long categoryId = 1;
        var request = new CategoryRequest(ParentCategoryId: null, Name: "Updated Name", Description: "Updated Desc");

        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Old",
            Description = "Old Desc",
            ParentCategoryId = null
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        MapperMock.Setup(m => m.Map(request, existingCategory));

        CategoryRepositoryMock
            .Setup(r => r.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Update failed"));

        // Act
        Func<Task> act = async () => await CategoryService.UpdateCategoryAsync(categoryId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Update failed");
    }
}