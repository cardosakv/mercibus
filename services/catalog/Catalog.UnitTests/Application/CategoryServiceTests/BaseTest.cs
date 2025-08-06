using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using MapsterMapper;
using Moq;

namespace Catalog.UnitTests.Application.CategoryServiceTests;

/// <summary>
/// Base class for unit tests in the category service.
/// </summary>
public abstract class BaseTest
{
    protected readonly ICategoryService CategoryService;

    protected readonly Mock<ICategoryRepository> CategoryRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IAppDbContext> DbContextMock;

    protected BaseTest()
    {
        CategoryRepositoryMock = new Mock<ICategoryRepository>();
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();

        CategoryService = new CategoryService(CategoryRepositoryMock.Object, MapperMock.Object, DbContextMock.Object);
    }
}