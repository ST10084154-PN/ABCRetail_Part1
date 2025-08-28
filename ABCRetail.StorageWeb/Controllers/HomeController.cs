using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;
using ABCRetail.StorageWeb.Models;

namespace ABCRetail.StorageWeb.Controllers;

public class HomeController : Controller
{
    private readonly TableStorageService _tables;
    private readonly QueueStorageService _queues;

    public HomeController(TableStorageService tables, QueueStorageService queues)
    {
        _tables = tables;
        _queues = queues;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Seed()
    {
        // Seed Customers
        for (int i = 1; i <= 5; i++)
        {
            await _tables.UpsertCustomerAsync(new CustomerProfile
            {
                FullName = $"Customer {i}",
                Email = $"customer{i}@abcretail.test",
                Address = $"123{i} Main Street"
            });
        }

        // Seed Products
        for (int i = 1; i <= 5; i++)
        {
            await _tables.UpsertProductAsync(new Product
            {
                Name = $"Product {i}",
                Description = $"Demo product {i}",
                Price = 50 + i,
                Stock = 10 * i
            });
        }

        await _queues.EnqueueAsync(new { kind = "seed", message = "Seeded demo data" });

        TempData["msg"] = "Seeded 5 Customers & 5 Products and pushed a queue message.";
        return RedirectToAction("Index");
    }
}