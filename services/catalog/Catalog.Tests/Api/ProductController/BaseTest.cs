using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Moq;

namespace Catalog.Tests.Api.ProductController;

/// <summary>
/// Base class for ProductController tests.
/// </summary>
public abstract class BaseTest
{
    protected readonly Catalog.Api.Controllers.ProductController ProductController;
    protected readonly Mock<IProductService> ProductServiceMock;

    protected BaseTest()
    {
        ProductServiceMock = new Mock<IProductService>();
        ProductController = new Catalog.Api.Controllers.ProductController(ProductServiceMock.Object);
    }
}