using Catalog.Api.Controllers;
using Catalog.Application.Interfaces.Services;
using Moq;

namespace Catalog.UnitTests.Api.ProductControllerTests;

/// <summary>
/// Base class for ProductControllerTests tests.
/// </summary>
public abstract class BaseTest
{
    protected readonly ProductController ProductController;
    protected readonly Mock<IProductService> ProductServiceMock;

    protected BaseTest()
    {
        ProductServiceMock = new Mock<IProductService>();
        ProductController = new ProductController(ProductServiceMock.Object);
    }
}