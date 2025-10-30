using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Payments.Application.DTOs;
using Payments.Application.Interfaces.Services;

namespace Payments.Infrastructure.Services;

public class XenditClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IPaymentClient
{
    public async Task<string> Initiate(PaymentClientRequest request, CancellationToken cancellationToken = default)
    {
        var apiKey = configuration["Xendit:ApiKey"];
        var base64ApiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:"));

        var httpClient = httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64ApiKey);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        
        var response = await httpClient.PostAsJsonAsync("https://api.xendit.co/sessions", request, options, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<PaymentClientError>(content, options);
            throw new Exception($"Payment Client API Error: {error?.ErrorCode} - {error?.Message}");
        }

        var result = JsonSerializer.Deserialize<PaymentClientResponse>(content, options);
        return result?.PaymentLinkUrl ?? throw new Exception("Payment link URL is null.");
    }
}