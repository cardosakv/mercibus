using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using MapsterMapper;
using Moq;

namespace Catalog.UnitTests.Application.BrandServiceTests;

/// <summary>
/// Base class for unit tests in the brand service.
/// </summary>
public abstract class BaseTest
{
    protected readonly IBrandService BrandService;

    protected readonly Mock<IBrandRepository> BrandRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IAppDbContext> DbContextMock;

    protected BaseTest()
    {
        BrandRepositoryMock = new Mock<IBrandRepository>();
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();

        BrandService = new BrandService(BrandRepositoryMock.Object, MapperMock.Object, DbContextMock.Object);
    }
}