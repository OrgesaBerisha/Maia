// Services/IStorageService.cs
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, string category);
    Task DeleteAsync(string fileName);
}

public class AzureStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureStorageService(IConfiguration config)
    {
        _blobServiceClient = new BlobServiceClient(
            config["AzureStorage:ConnectionString"]);
        _containerName = config["AzureStorage:ContainerName"]!;
    }

    public async Task<string> UploadAsync(IFormFile file, string category)
    {
        var containerClient = _blobServiceClient
            .GetBlobContainerClient(_containerName);

        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileName = $"{category}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blobClient = containerClient.GetBlobClient(fileName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders
        {
            ContentType = file.ContentType
        });

        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string fileName)
    {
        var containerClient = _blobServiceClient
            .GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}