using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using MapsterMapper;
using Moq;

namespace Catalog.UnitTests.Application.ProductImageServiceTests;

/// <summary>
///     Base class for unit tests in the product image service.
/// </summary>
public abstract class BaseTest
{
    protected readonly IProductImageService ProductImageService;

    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IProductImageRepository> ProductImageRepositoryMock;
    protected readonly Mock<IBlobStorageService> BlobStorageServiceMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IAppDbContext> DbContextMock;

    protected BaseTest()
    {
        ProductRepositoryMock = new Mock<IProductRepository>();
        ProductImageRepositoryMock = new Mock<IProductImageRepository>();
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();
        BlobStorageServiceMock = new Mock<IBlobStorageService>();

        ProductImageService = new ProductImageService(ProductRepositoryMock.Object, ProductImageRepositoryMock.Object, DbContextMock.Object, BlobStorageServiceMock.Object, MapperMock.Object);
    }
}