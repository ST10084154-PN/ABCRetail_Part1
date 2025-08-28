using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ABCRetail.StorageWeb.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly StorageOptions _options;

    public BlobStorageService(BlobServiceClient blobServiceClient, StorageOptions options)
    {
        _blobServiceClient = blobServiceClient;
        _options = options;
    }

    public BlobContainerClient GetContainer()
    {
        var container = _blobServiceClient.GetBlobContainerClient(_options.BlobContainer);
        container.CreateIfNotExists(PublicAccessType.Blob);
        return container;
    }

    public async Task<string> UploadAsync(string fileName, Stream stream, string contentType)
    {
        var container = GetContainer();
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });
        return blob.Uri.ToString();
    }

    public IEnumerable<BlobItem> ListBlobs()
    {
        var container = GetContainer();
        return container.GetBlobs();
    }
}