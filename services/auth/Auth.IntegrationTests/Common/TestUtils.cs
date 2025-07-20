using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Azure.Storage.Blobs;
using Mercibus.Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

    private static FormFile CreateDummyFormFile(string fileName = "test.jpg", string contentType = "image/jpeg")
    {
        // JPEG header (FFD8) and footer (FFD9)
        var imageBytes = new byte[]
        {
            0xFF, 0xD8, // JPEG SOI marker
            0xFF, 0xE0, 0x00, 0x10, // APP0 marker
            0x4A, 0x46, 0x49, 0x46, // 'JFIF'
            0x00, // '\0'
            0xFF, 0xD9 // JPEG EOI marker
        };

        var stream = new MemoryStream(imageBytes);
        return new FormFile(stream, 0, stream.Length, "image/jpeg", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    public static MultipartFormDataContent CreateDummyImageForm()
    {
        var dummyFile = CreateDummyFormFile("upload_sample.jpg");
        var form = new MultipartFormDataContent();
        var streamContent = new StreamContent(dummyFile.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(dummyFile.ContentType);
        form.Add(streamContent, "image", dummyFile.FileName);

        return form;
    }

    public static async Task UploadBlobAsync(IConfiguration configuration, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(configuration["ConnectionStrings:BlobStorageConnection"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(Constants.BlobStorageContainerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(new MemoryStream([0x1, 0x2, 0x3]), overwrite: true);
    }
}