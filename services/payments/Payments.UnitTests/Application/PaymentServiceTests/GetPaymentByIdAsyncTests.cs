using FluentAssertions;
using Moq;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.UnitTests.Application.PaymentServiceTests;

public class GetPaymentByIdAsyncTests : BaseTest
{
    [Fact]
    public async Task Success_WhenPaymentExists()
    {
        // Arrange
        long paymentId = 1;
        var payment = new Payment
        {
            Id = paymentId,
            OrderId = 10,
            CustomerId = "cust-1",
            Amount = 1000,
            Currency = "PHP",
            Status = PaymentStatus.Completed
        };

        var mapped = new PaymentResponse(
            payment.Id,
            payment.OrderId,
            payment.CustomerId,
            payment.Amount,
            payment.Currency,
            payment.Status.ToString(),
            DateTime.UtcNow
        );

        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(paymentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        MapperMock.Setup(m => m.Map<PaymentResponse>(payment)).Returns(mapped);

        // Act
        var result = await PaymentService.GetPaymentByIdAsync(paymentId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeOfType<PaymentResponse>();
        ((PaymentResponse)result.Data!).Status.Should().Be("Completed");
    }

    [Fact]
    public async Task ReturnsError_WhenPaymentNotFound()
    {
        // Arrange
        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        // Act
        var result = await PaymentService.GetPaymentByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Throws_WhenRepositoryThrows()
    {
        // Arrange
        PaymentRepositoryMock.Setup(r => r.GetPaymentByIdAsync(1, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB unavailable"));

        // Act
        Func<Task> act = async () => await PaymentService.GetPaymentByIdAsync(1);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("DB unavailable");
    }
}