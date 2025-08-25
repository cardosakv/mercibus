using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using MapsterMapper;
using Moq;

namespace Catalog.UnitTests.Application.ProductReviewServiceTests;

/// <summary>
///     Base class for unit tests in the product review service.
/// </summary>
public abstract class BaseTest
{
    protected readonly ProductReviewService ProductReviewService;
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IProductReviewRepository> ProductReviewRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IAppDbContext> DbContextMock;
    protected readonly Mock<ICacheService> CacheServiceMock;

    protected BaseTest()
    {
        ProductRepositoryMock = new Mock<IProductRepository>();
        ProductReviewRepositoryMock = new Mock<IProductReviewRepository>();
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();
        CacheServiceMock = new Mock<ICacheService>();

        ProductReviewService = new ProductReviewService(ProductReviewRepositoryMock.Object, ProductRepositoryMock.Object, MapperMock.Object, DbContextMock.Object, CacheServiceMock.Object);
    }
}