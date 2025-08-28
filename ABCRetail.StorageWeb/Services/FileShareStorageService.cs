using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.StorageWeb.Services;

public class FileShareStorageService
{
    private readonly ShareServiceClient _shareServiceClient;
    private readonly StorageOptions _options;

    public FileShareStorageService(ShareServiceClient shareServiceClient, StorageOptions options)
    {
        _shareServiceClient = shareServiceClient;
        _options = options;
    }

    public ShareClient GetShare()
    {
        var share = _shareServiceClient.GetShareClient(_options.ContractsShare);
        share.CreateIfNotExists();
        return share;
    }

    public async Task UploadAsync(string fileName, Stream content)
    {
        var share = GetShare();
        var root = share.GetRootDirectoryClient();
        await root.CreateIfNotExistsAsync();
        var file = root.GetFileClient(fileName);
        content.Position = 0;
        await file.CreateAsync(content.Length);
        await file.UploadAsync(content);
    }

    public IEnumerable<ShareFileItem> ListFiles()
    {
        var share = GetShare();
        var root = share.GetRootDirectoryClient();
        return root.GetFilesAndDirectories().Where(f => !f.IsDirectory).Select(f => f);
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        var share = GetShare();
        var root = share.GetRootDirectoryClient();
        var file = root.GetFileClient(fileName);
        var resp = await file.DownloadAsync();
        var ms = new MemoryStream();
        await resp.Value.Content.CopyToAsync(ms);
        ms.Position = 0;
        return ms;
    }
}