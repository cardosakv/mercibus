using MapsterMapper;
using Moq;
using Orders.Application.Interfaces.Messaging;
using Orders.Application.Interfaces.Repositories;
using Orders.Application.Interfaces.Services;
using Orders.Application.Services;

namespace Orders.UnitTests.Application.OrderServiceTests;

public abstract class BaseTest
{
    protected readonly Mock<IAppDbContext> DbContextMock;
    protected readonly Mock<IEventPublisher> EventPublisherMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly IOrderService OrderService;

    protected BaseTest()
    {
        MapperMock = new Mock<IMapper>();
        DbContextMock = new Mock<IAppDbContext>();
        OrderRepositoryMock = new Mock<IOrderRepository>();
        EventPublisherMock = new Mock<IEventPublisher>();

        OrderService = new OrderService(
            MapperMock.Object,
            DbContextMock.Object,
            OrderRepositoryMock.Object,
            EventPublisherMock.Object
        );
    }
}