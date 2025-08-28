using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;

namespace ABCRetail.StorageWeb.Controllers;

public class QueuesController : Controller
{
    private readonly QueueStorageService _queues;
    public QueuesController(QueueStorageService queues) => _queues = queues;

    public IActionResult Index()
    {
        var messages = _queues.PeekMessages(16).ToList();
        return View(messages);
    }
}