using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Catalog.Application.Common;
using Catalog.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure.Services;

public class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    public async Task<string> UploadFileAsync(string fileName, Stream fileStream)
    {
        var blobContainerClient =
            new BlobContainerClient(
                connectionString: configuration["ConnectionStrings:BlobStorageConnection"],
                Constants.BlobStorage.ProductImagesContainer);
        await blobContainerClient.CreateIfNotExistsAsync();

        var bobClient = blobContainerClient.GetBlobClient(fileName);
        await bobClient.UploadAsync(fileStream, overwrite: true);

        return bobClient.Uri.ToString();
    }

    public Task<string> GenerateBlobUrlAsync(string blobName, DateTimeOffset expiryTime)
    {
        var blobServiceClient = new BlobServiceClient(configuration["ConnectionStrings:BlobStorageConnection"]);
        var containerClient = blobServiceClient.GetBlobContainerClient(Constants.BlobStorage.ProductImagesContainer);
        var blobClient = containerClient.GetBlobClient(blobName);

        var builder = new BlobSasBuilder
        {
            BlobContainerName = Constants.BlobStorage.ProductImagesContainer,
            BlobName = blobName,
            ExpiresOn = expiryTime,
            Resource = "b" // 'b' for blob
        };

        builder.SetPermissions(BlobSasPermissions.Read);

        var sasUrl = blobClient.GenerateSasUri(builder).ToString();

        return Task.FromResult(sasUrl);
    }

    public async Task<bool> DeleteBlobAsync(string blobName)
    {
        var blobContainerClient =
            new BlobContainerClient(
                connectionString: configuration["ConnectionStrings:BlobStorageConnection"],
                Constants.BlobStorage.ProductImagesContainer);

        var blobClient = blobContainerClient.GetBlobClient(blobName);
        return await blobClient.DeleteIfExistsAsync();
    }
}