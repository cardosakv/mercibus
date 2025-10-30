using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mercibus.Common.Responses;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using Payments.IntegrationTests.Common;

namespace Payments.IntegrationTests.PaymentTests;

public class InitiatePaymentAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/payments/initiate";

    [Fact]
    public async Task ReturnsOk_WhenInitiationSucceeds()
    {
        // Arrange
        var db = factory.CreateDbContext();
        var payment = await db.Payments.AddAsync(
            new Payment
            {
                OrderId = 10,
                CustomerId = "cust-1",
                Amount = 500,
                Currency = "PHP",
                Status = PaymentStatus.AwaitingUserAction
            });
        await db.SaveChangesAsync();

        var billing = new BillingRequest("John", "Doe", "john@example.com", null, "Street", null, "City", "State", 1111, "PH");
        var request = new PaymentRequest(payment.Entity.OrderId, billing);

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenPaymentNotFound()
    {
        // Arrange
        var billing = new BillingRequest("A", "B", "a@b.com", null, "Street", null, "City", "State", 1111, "PH");
        var request = new PaymentRequest(999, billing);
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenBillingRequestIsInvalid()
    {
        // Arrange
        var db = factory.CreateDbContext();
        var payment = await db.Payments.AddAsync(
            new Payment
            {
                OrderId = 15,
                CustomerId = "cust-1",
                Amount = 500,
                Currency = "PHP",
                Status = PaymentStatus.AwaitingUserAction
            });
        await db.SaveChangesAsync();

        var billing = new BillingRequest(
            null!, // invalid
            null!, // invalid
            "", // invalid
            null,
            "", // invalid
            null,
            "", // invalid
            "", // invalid
            0, // invalid
            "" // invalid
        );

        var request = new PaymentRequest(payment.Entity.OrderId, billing);

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.PostAsJsonAsync(Url, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        result.Should().NotBeNull();
        result!.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("validation_failed");
        result.Error.Params.Should().Contain(p => p.Field == "firstName");
        result.Error.Params.Should().Contain(p => p.Field == "email");
    }
}