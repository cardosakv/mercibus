using System.Text.Json.Serialization;

namespace Payments.Application.DTOs;

/// <summary>
/// Represents a payment webhook request.
/// </summary>
public class PaymentWebhookRequest
{
    [JsonPropertyName("data")]
    public PaymentWebhookData Data { get; set; } = new();
}

public class PaymentWebhookData
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("reference_id")]
    public string ReferenceId { get; set; } = string.Empty;
}