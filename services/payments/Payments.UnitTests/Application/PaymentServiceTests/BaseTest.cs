using MapsterMapper;
using Moq;
using Payments.Application.Interfaces.Messaging;
using Payments.Application.Interfaces.Repositories;
using Payments.Application.Interfaces.Services;
using Payments.Application.Services;

namespace Payments.UnitTests.Application.PaymentServiceTests;

public abstract class BaseTest
{
    protected readonly Mock<IPaymentRepository> PaymentRepositoryMock;
    protected readonly Mock<IAppDbContext> DbContextMock;
    protected readonly Mock<IEventPublisher> EventPublisherMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IPaymentClient> PaymentClientMock;
    protected readonly IPaymentService PaymentService;

    protected BaseTest()
    {
        PaymentRepositoryMock = new Mock<IPaymentRepository>();
        DbContextMock = new Mock<IAppDbContext>();
        EventPublisherMock = new Mock<IEventPublisher>();
        MapperMock = new Mock<IMapper>();
        PaymentClientMock = new Mock<IPaymentClient>();

        PaymentService = new PaymentService(
            PaymentClientMock.Object,
            PaymentRepositoryMock.Object,
            DbContextMock.Object,
            MapperMock.Object,
            EventPublisherMock.Object
        );
    }
}