using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ABCRetail.StorageWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Bind storage settings
var storageCfg = builder.Configuration.GetSection("AzureStorage");

builder.Services.AddSingleton(new TableServiceClient(builder.Configuration["AzureStorage:ConnectionString"]));
builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration["AzureStorage:ConnectionString"]));
builder.Services.AddSingleton(new QueueServiceClient(builder.Configuration["AzureStorage:ConnectionString"]));
builder.Services.AddSingleton(new ShareServiceClient(builder.Configuration["AzureStorage:ConnectionString"]));

builder.Services.AddSingleton<StorageOptions>(sp => new StorageOptions
{
    CustomersTable = builder.Configuration["AzureStorage:CustomersTable"] ?? "CustomerProfiles",
    ProductsTable = builder.Configuration["AzureStorage:ProductsTable"] ?? "Products",
    BlobContainer = builder.Configuration["AzureStorage:BlobContainer"] ?? "product-images",
    OrdersQueue = builder.Configuration["AzureStorage:OrdersQueue"] ?? "orders-queue",
    ContractsShare = builder.Configuration["AzureStorage:ContractsShare"] ?? "contracts"
});

builder.Services.AddScoped<TableStorageService>();
builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddScoped<QueueStorageService>();
builder.Services.AddScoped<FileShareStorageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();