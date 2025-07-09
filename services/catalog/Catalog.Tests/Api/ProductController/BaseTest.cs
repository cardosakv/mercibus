using Catalog.Application.Services;
using Moq;

namespace Catalog.Tests.Api.ProductController;

/// <summary>
/// Base class for ProductController tests.
/// </summary>
public abstract class BaseTest
{
    protected readonly Catalog.Api.Controllers.ProductController ProductController;
    protected readonly Mock<ProductService> ProductServiceMock;

    protected BaseTest()
    {
        ProductServiceMock = new Mock<ProductService>();
        ProductController = new Catalog.Api.Controllers.ProductController(ProductServiceMock.Object);
    }
}