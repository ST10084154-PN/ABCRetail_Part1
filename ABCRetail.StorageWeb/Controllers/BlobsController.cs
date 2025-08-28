using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;

namespace ABCRetail.StorageWeb.Controllers;

public class BlobsController : Controller
{
    private readonly BlobStorageService _blobs;
    private readonly QueueStorageService _queues;

    public BlobsController(BlobStorageService blobs, QueueStorageService queues)
    {
        _blobs = blobs;
        _queues = queues;
    }

    public IActionResult Index()
    {
        var items = _blobs.ListBlobs().Select(b => b.Name).ToList();
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            TempData["msg"] = "Please select a file.";
            return RedirectToAction(nameof(Index));
        }

        using var stream = file.OpenReadStream();
        var url = await _blobs.UploadAsync(file.FileName, stream, file.ContentType);
        await _queues.EnqueueAsync(new { kind = "upload", imageName = file.FileName, url, msg = "Uploading image" });
        TempData["msg"] = $"Uploaded {file.FileName}";
        return RedirectToAction(nameof(Index));
    }
}