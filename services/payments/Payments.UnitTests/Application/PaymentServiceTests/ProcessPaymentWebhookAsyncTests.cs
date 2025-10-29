using FluentAssertions;
using Messaging.Events;
using Moq;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.UnitTests.Application.PaymentServiceTests;

public class ProcessPaymentWebhookAsyncTests : BaseTest
{
    [Fact]
    public async Task ReturnsError_WhenReferenceIdIsInvalid()
    {
        // Arrange
        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = "abc",
                Status = "SUCCEEDED"
            }
        };

        // Act
        var result = await PaymentService.ProcessPaymentWebhookAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnsError_WhenPaymentNotFound()
    {
        // Arrange
        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = "100",
                Status = "FAILED"
            }
        };
        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(100, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        // Act
        var result = await PaymentService.ProcessPaymentWebhookAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task PublishesSucceededEvent_WhenStatusIsSuccess()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            CustomerId = "cust-1",
            Currency = "PHP",
            Amount = 500,
            Status = PaymentStatus.Processing
        };
        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = "1",
                Status = "SUCCEEDED"
            }
        };

        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        DbContextMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        EventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<PaymentSucceeded>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await PaymentService.ProcessPaymentWebhookAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        EventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<PaymentSucceeded>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishesFailedEvent_WhenStatusIsFailed()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 2,
            OrderId = 20,
            CustomerId = "cust-2",
            Currency = "PHP",
            Amount = 800,
            Status = PaymentStatus.Processing
        };
        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = "2",
                Status = "FAILED"
            }
        };

        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        DbContextMock.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        EventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<PaymentFailed>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await PaymentService.ProcessPaymentWebhookAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        EventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<PaymentFailed>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}