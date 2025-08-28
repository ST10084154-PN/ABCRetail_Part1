using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;

namespace ABCRetail.StorageWeb.Controllers;

public class FilesController : Controller
{
    private readonly FileShareStorageService _files;

    public FilesController(FileShareStorageService files)
    {
        _files = files;
    }

    public IActionResult Index()
    {
        var files = _files.ListFiles().Select(f => f.Name).ToList();
        return View(files);
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
        await _files.UploadAsync(file.FileName, stream);
        TempData["msg"] = $"Uploaded {file.FileName}";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Download(string name)
    {
        var stream = await _files.DownloadAsync(name);
        return File(stream, "application/octet-stream", name);
    }
}