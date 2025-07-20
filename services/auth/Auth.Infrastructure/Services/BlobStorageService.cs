using Auth.Application.Common;
using Auth.Application.Interfaces.Services;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Services;

public class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        var blobContainerClient =
            new BlobContainerClient(configuration["ConnectionStrings:BlobStorageConnection"], Constants.BlobStorageContainerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        var bobClient = blobContainerClient.GetBlobClient(fileName);
        await bobClient.UploadAsync(fileStream, overwrite: true);

        return bobClient.Uri.ToString();
    }

    public Task<string> GenerateBlobUrlAsync(string blobName, DateTimeOffset expiryTime)
    {
        var blobServiceClient = new BlobServiceClient(configuration["ConnectionStrings:BlobStorageConnection"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(Constants.BlobStorageContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        var builder = new BlobSasBuilder
        {
            BlobContainerName = Constants.BlobStorageContainerName,
            BlobName = blobName,
            ExpiresOn = expiryTime,
            Resource = "b" // 'b' for blob
        };

        builder.SetPermissions(BlobSasPermissions.Read);

        var storageSharedKeyCredential = new StorageSharedKeyCredential(
            configuration["BlobStorage:AccountName"],
            configuration["BlobStorage:AccountKey"]
        );

        var sasToken = builder.ToSasQueryParameters(storageSharedKeyCredential).ToString();
        var sasUrl = $"{blobClient.Uri}?{sasToken}";

        return Task.FromResult(sasUrl);
    }
}