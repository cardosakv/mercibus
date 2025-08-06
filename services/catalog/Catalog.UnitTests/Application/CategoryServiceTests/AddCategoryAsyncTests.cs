using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

public class AddCategoryAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenCategoryIsAdded_WithParent()
    {
        // Arrange
        var request = new CategoryRequest(
            ParentCategoryId: 5,
            Name: "Smartphones",
            Description: "Mobile phones and accessories"
        );

        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId,
            CreatedAt = DateTime.UtcNow
        };

        var savedCategory = entity;
        savedCategory.Id = 12;

        MapperMock.Setup(m => m.Map<Category>(request)).Returns(entity);
        CategoryRepositoryMock.Setup(r => r.AddCategoryAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedCategory);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock.Setup(m => m.Map<CategoryResponse>(savedCategory))
            .Returns(
                new CategoryResponse(
                    Id: 12,
                    Name: "Smartphones",
                    Description: "Mobile phones and accessories",
                    ParentCategoryId: 5
                )
            );

        // Act
        var result = await CategoryService.AddCategoryAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Data.Should().BeOfType<CategoryResponse>().Subject;
        data.Name.Should().Be(request.Name);
        data.Description.Should().Be(request.Description);
        data.ParentCategoryId.Should().Be(request.ParentCategoryId);
    }

    [Fact]
    public async Task Success_WhenCategoryIsAdded_WithoutParent()
    {
        // Arrange
        var request = new CategoryRequest(
            ParentCategoryId: null,
            Name: "Electronics",
            Description: "All electronic products"
        );

        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = null,
            CreatedAt = DateTime.UtcNow
        };

        var savedCategory = entity;
        savedCategory.Id = 7;

        MapperMock.Setup(m => m.Map<Category>(request)).Returns(entity);
        CategoryRepositoryMock.Setup(r => r.AddCategoryAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedCategory);
        DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        MapperMock.Setup(m => m.Map<CategoryResponse>(savedCategory))
            .Returns(
                new CategoryResponse(
                    Id: 7,
                    Name: "Electronics",
                    Description: "All electronic products",
                    ParentCategoryId: null
                )
            );

        // Act
        var result = await CategoryService.AddCategoryAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var data = result.Data.Should().BeOfType<CategoryResponse>().Subject;
        data.Name.Should().Be(request.Name);
        data.Description.Should().Be(request.Description);
        data.ParentCategoryId.Should().BeNull();
    }

    [Fact]
    public async Task Throws_WhenAddFails()
    {
        // Arrange
        var request = new CategoryRequest(
            ParentCategoryId: 1,
            Name: "Faulty Category",
            Description: "Should fail"
        );

        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = 1,
            CreatedAt = DateTime.UtcNow
        };

        MapperMock.Setup(m => m.Map<Category>(request)).Returns(entity);
        CategoryRepositoryMock
            .Setup(r => r.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Insert failed"));

        // Act
        Func<Task> act = async () => await CategoryService.AddCategoryAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insert failed");
    }
}