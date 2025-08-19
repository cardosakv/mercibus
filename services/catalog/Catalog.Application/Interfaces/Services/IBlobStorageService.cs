namespace Catalog.Application.Interfaces.Services;

/// <summary>
/// Interface for a service that handles blob storage operations.
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Uploads a file to the blob storage.
    /// </summary>
    /// <param name="fileName">The name of the file to upload.</param>
    /// <param name="fileStream">The stream containing the file data.</param>
    /// <returns>The URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(string fileName, Stream fileStream);

    /// <summary>
    /// Generates a URL for accessing a blob in the storage with an expiry time.
    /// </summary>
    /// <param name="blobName">The name of the blob.</param>
    /// <param name="expiryTime">Expiry time of the url.</param>
    /// <returns>Publicly accessible blob url.</returns>
    Task<string> GenerateBlobUrlAsync(string blobName, DateTimeOffset expiryTime);

    /// <summary>
    /// Deletes a blob from the storage.
    /// </summary>
    /// <param name="blobName">The name of the blob.</param>
    /// <returns>Boolean indicating success or failure.</returns>
    Task<bool> DeleteBlobAsync(string blobName);
}