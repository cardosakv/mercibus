using FluentAssertions;
using Messaging.Events;
using Moq;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.UnitTests.Application.PaymentServiceTests;

public class InitiatePaymentAsyncTests : BaseTest
{
    [Fact]
    public async Task ReturnsError_WhenPaymentNotFound()
    {
        // Arrange
        var request = new PaymentRequest(123, new BillingRequest("A", "B", "a@b.com", null, "Street", null, "City", "State", 1111, "PH"));
        PaymentRepositoryMock.Setup(r => r.GetPaymentByOrderIdAsync(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        // Act
        var result = await PaymentService.InitiatePaymentAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Theory]
    [InlineData(PaymentStatus.Processing, "PaymentCurrentlyProcessing")]
    [InlineData(PaymentStatus.Completed, "PaymentAlreadyCompleted")]
    [InlineData(PaymentStatus.Failed, "PaymentFailed")]
    public async Task ReturnsError_WhenPaymentInInvalidState(PaymentStatus status, string expectedCode)
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            CustomerId = "cust-1",
            Currency = "PHP",
            Amount = 1000,
            Status = status
        };

        PaymentRepositoryMock.Setup(r => r.GetPaymentByOrderIdAsync(payment.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        var request = new PaymentRequest(payment.OrderId, new BillingRequest("A", "B", "a@b.com", null, "Street", null, "City", "State", 1111, "PH"));

        // Act
        var result = await PaymentService.InitiatePaymentAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Success_WhenPaymentInitiationSucceeds()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            CustomerId = "cust-1",
            Currency = "PHP",
            Amount = 500,
            Status = PaymentStatus.AwaitingUserAction
        };

        var request = new PaymentRequest(payment.OrderId, new BillingRequest("A", "B", "a@b.com", null, "Street", null, "City", "State", 1111, "PH"));

        PaymentRepositoryMock.Setup(r => r.GetPaymentByOrderIdAsync(payment.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        PaymentClientMock.Setup(c => c.Initiate(It.IsAny<PaymentClientRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://pay.link");
        PaymentRepositoryMock.Setup(r => r.UpdatePaymentAsync(payment, It.IsAny<CancellationToken>()));
        DbContextMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await PaymentService.InitiatePaymentAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("https://pay.link");
    }

    [Fact]
    public async Task PublishesEvent_WhenPaymentClientThrows()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            CustomerId = "cust-1",
            Currency = "PHP",
            Amount = 500,
            Status = PaymentStatus.AwaitingUserAction
        };

        var request = new PaymentRequest(payment.OrderId, new BillingRequest("A", "B", "a@b.com", null, "Street", null, "City", "State", 1111, "PH"));

        PaymentRepositoryMock.Setup(r => r.GetPaymentByOrderIdAsync(payment.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        PaymentClientMock.Setup(c => c.Initiate(It.IsAny<PaymentClientRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("gateway failed"));

        EventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<PaymentFailed>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await PaymentService.InitiatePaymentAsync(request);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("gateway failed");
        EventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<PaymentFailed>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}