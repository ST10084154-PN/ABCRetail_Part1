using Azure;
using Azure.Data.Tables;
using ABCRetail.StorageWeb.Models;

namespace ABCRetail.StorageWeb.Services;

public class TableStorageService
{
    private readonly TableServiceClient _tableServiceClient;
    private readonly StorageOptions _options;

    public TableStorageService(TableServiceClient tableServiceClient, StorageOptions options)
    {
        _tableServiceClient = tableServiceClient;
        _options = options;
    }

    public TableClient GetCustomersTable()
    {
        var table = _tableServiceClient.GetTableClient(_options.CustomersTable);
        table.CreateIfNotExists();
        return table;
    }

    public TableClient GetProductsTable()
    {
        var table = _tableServiceClient.GetTableClient(_options.ProductsTable);
        table.CreateIfNotExists();
        return table;
    }

    public async Task UpsertCustomerAsync(CustomerProfile customer)
    {
        var table = GetCustomersTable();
        await table.UpsertEntityAsync(customer);
    }

    public async Task UpsertProductAsync(Product product)
    {
        var table = GetProductsTable();
        await table.UpsertEntityAsync(product);
    }

    public IEnumerable<CustomerProfile> GetCustomers()
    {
        var table = GetCustomersTable();
        return table.Query<CustomerProfile>(c => c.PartitionKey == "CUSTOMER");
    }

    public IEnumerable<Product> GetProducts()
    {
        var table = GetProductsTable();
        return table.Query<Product>(p => p.PartitionKey == "PRODUCT");
    }
}