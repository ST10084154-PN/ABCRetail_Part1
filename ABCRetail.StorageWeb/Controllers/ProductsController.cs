using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;
using ABCRetail.StorageWeb.Models;

namespace ABCRetail.StorageWeb.Controllers;

public class ProductsController : Controller
{
    private readonly TableStorageService _tables;
    private readonly QueueStorageService _queues;

    public ProductsController(TableStorageService tables, QueueStorageService queues)
    {
        _tables = tables;
        _queues = queues;
    }

    public IActionResult Index()
    {
        var products = _tables.GetProducts().ToList();
        return View(products);
    }

    [HttpGet]
    public IActionResult Create() => View(new Product());

    [HttpPost]
    public async Task<IActionResult> Create(Product model, int orderQuantity = 0)
    {
        if (!ModelState.IsValid) return View(model);
        await _tables.UpsertProductAsync(model);

        if (orderQuantity > 0)
        {
            await _queues.EnqueueAsync(new { kind = "order", productId = model.RowKey, qty = orderQuantity, msg = "Processing order" });
        }

        return RedirectToAction(nameof(Index));
    }
}