using Microsoft.AspNetCore.Mvc;
using ABCRetail.StorageWeb.Services;
using ABCRetail.StorageWeb.Models;

namespace ABCRetail.StorageWeb.Controllers;

public class CustomersController : Controller
{
    private readonly TableStorageService _tables;
    public CustomersController(TableStorageService tables) => _tables = tables;

    public IActionResult Index()
    {
        var customers = _tables.GetCustomers().ToList();
        return View(customers);
    }

    [HttpGet]
    public IActionResult Create() => View(new CustomerProfile());

    [HttpPost]
    public async Task<IActionResult> Create(CustomerProfile model)
    {
        if (!ModelState.IsValid) return View(model);
        await _tables.UpsertCustomerAsync(model);
        return RedirectToAction(nameof(Index));
    }
}