namespace ABCRetail.StorageWeb.Services;

public class StorageOptions
{
    public string CustomersTable { get; set; } = "CustomerProfiles";
    public string ProductsTable { get; set; } = "Products";
    public string BlobContainer { get; set; } = "product-images";
    public string OrdersQueue { get; set; } = "orders-queue";
    public string ContractsShare { get; set; } = "contracts";
}