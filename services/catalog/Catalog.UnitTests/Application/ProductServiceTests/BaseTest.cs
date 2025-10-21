using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Messaging;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using MapsterMapper;
using Moq;

namespace Catalog.UnitTests.Application.ProductServiceTests;

/// <summary>
/// Base class for unit tests in the product service.
/// </summary>
public abstract class BaseTest
{
    protected readonly IProductService ProductService;

    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IBlobStorageService> BlobStorageServiceMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IAppDbContext> DbContextMock;
    protected readonly Mock<ICacheService> CacheServiceMock;
    protected readonly Mock<IEventPublisher> EventPublisherMock;

    protected BaseTest()
    {
        ProductRepositoryMock = new Mock<IProductRepository>();
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();
        BlobStorageServiceMock = new Mock<IBlobStorageService>();
        CacheServiceMock = new Mock<ICacheService>();
        EventPublisherMock = new Mock<IEventPublisher>();

        ProductService = new ProductService(
            ProductRepositoryMock.Object,
            BlobStorageServiceMock.Object,
            MapperMock.Object,
            DbContextMock.Object,
            CacheServiceMock.Object,
            EventPublisherMock.Object
        );
    }
}