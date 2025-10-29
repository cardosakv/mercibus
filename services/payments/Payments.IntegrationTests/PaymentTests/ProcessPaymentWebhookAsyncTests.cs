using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using Payments.IntegrationTests.Common;

namespace Payments.IntegrationTests.PaymentTests;

public class ProcessPaymentWebhookAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/payments/webhook";

    [Fact]
    public async Task ReturnsOk_WhenWebhookMarksPaymentCompleted()
    {
        // Arrange
        var db = factory.CreateDbContext();
        var payment = await db.Payments.AddAsync(
            new Payment
            {
                OrderId = 10,
                CustomerId = "cust-1",
                Amount = 1000,
                Currency = "PHP",
                Status = PaymentStatus.Processing
            });
        await db.SaveChangesAsync();

        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = payment.Entity.Id.ToString(),
                Status = "SUCCEEDED"
            }
        };

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await factory.CreateDbContext().Payments.FindAsync(payment.Entity.Id);
        updated!.Status.Should().Be(PaymentStatus.Completed);
    }

    [Fact]
    public async Task ReturnsOk_WhenWebhookMarksPaymentFailed()
    {
        // Arrange
        var db = factory.CreateDbContext();
        var payment = await db.Payments.AddAsync(
            new Payment
            {
                OrderId = 20,
                CustomerId = "cust-2",
                Amount = 500,
                Currency = "PHP",
                Status = PaymentStatus.Processing
            });
        await db.SaveChangesAsync();

        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = payment.Entity.Id.ToString(),
                Status = "FAILED"
            }
        };

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await factory.CreateDbContext().Payments.FindAsync(payment.Entity.Id);
        updated!.Status.Should().Be(PaymentStatus.Failed);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenReferenceIdInvalid()
    {
        // Arrange
        var request = new PaymentWebhookRequest
        {
            Data = new PaymentWebhookData
            {
                ReferenceId = "invalid-id",
                Status = "SUCCEEDED"
            }
        };

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}