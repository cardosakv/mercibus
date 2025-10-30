using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mercibus.Common.Responses;
using Newtonsoft.Json;
using Payments.Application.DTOs;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using Payments.IntegrationTests.Common;

namespace Payments.IntegrationTests.PaymentTests;

public class GetPaymentByIdAsyncTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private const string Url = "api/payments/";

    [Fact]
    public async Task ReturnsOk_WhenPaymentExists()
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
                Status = PaymentStatus.Completed
            });
        await db.SaveChangesAsync();

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.GetAsync(Url + payment.Entity.Id);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<ApiSuccessResponse>();
        var result = JsonConvert.DeserializeObject<PaymentResponse>(content!.Data!.ToString()!);
        result!.Id.Should().Be(payment.Entity.Id);
        result.Status.Should().Be("Completed");
    }

    [Fact]
    public async Task ReturnsBadRequest_WhenPaymentNotFound()
    {
        // Arrange
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer test-token");

        // Act
        var response = await client.GetAsync(Url + 9999);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}