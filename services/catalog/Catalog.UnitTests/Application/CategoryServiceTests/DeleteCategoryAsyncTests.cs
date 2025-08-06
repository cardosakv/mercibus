using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

public class DeleteCategoryAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenCategoryIsDeleted()
    {
        // Arrange
        const long categoryId = 1;

        var category = new Category
        {
            Id = categoryId,
            Name = "To Delete",
            Description = "Category to be deleted"
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        CategoryRepositoryMock
            .Setup(r => r.IsCategoryUsedInProductsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        CategoryRepositoryMock
            .Setup(r => r.DeleteCategoryAsync(category, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        DbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await CategoryService.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
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
        var result = await CategoryService.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnsError_WhenCategoryIsInUse()
    {
        // Arrange
        long categoryId = 10;

        var category = new Category
        {
            Id = categoryId,
            Name = "Linked Category"
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        CategoryRepositoryMock
            .Setup(r => r.IsCategoryUsedInProductsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await CategoryService.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenDeleteFails()
    {
        // Arrange
        long categoryId = 5;

        var category = new Category
        {
            Id = categoryId,
            Name = "Failing Delete"
        };

        CategoryRepositoryMock
            .Setup(r => r.GetCategoryByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        CategoryRepositoryMock
            .Setup(r => r.IsCategoryUsedInProductsAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        CategoryRepositoryMock
            .Setup(r => r.DeleteCategoryAsync(category, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Delete failed"));

        // Act
        Func<Task> act = async () => await CategoryService.DeleteCategoryAsync(categoryId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Delete failed");
    }
}