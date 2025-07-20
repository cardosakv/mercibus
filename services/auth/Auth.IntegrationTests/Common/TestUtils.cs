using System.Net.Http.Json;
using Auth.Application.DTOs;
using Mercibus.Common.Responses;
using Newtonsoft.Json;

namespace Auth.IntegrationTests.Common;

/// <summary>
/// Utility class for common operations in integration tests.
/// </summary>
public static class TestUtils
{
    public static async Task<AuthTokenResponse?> LoginUser(HttpClient client, string url, string username, string password)
    {
        var loginRequest = new LoginRequest { Username = username, Password = password };
        var loginResponse = await client.PostAsJsonAsync(url, loginRequest);

        loginResponse.EnsureSuccessStatusCode();

        var contentString = await loginResponse.Content.ReadAsStringAsync();
        var loginContent = JsonConvert.DeserializeObject<ApiSuccessResponse>(contentString);

        if (loginContent?.Data is null)
        {
            throw new InvalidOperationException("Login response missing expected data");
        }

        return JsonConvert.DeserializeObject<AuthTokenResponse>(loginContent.Data.ToString()!);
    }
}