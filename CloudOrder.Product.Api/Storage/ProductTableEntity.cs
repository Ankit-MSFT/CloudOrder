using Azure;
using Azure.Data.Tables;

namespace CloudOrder.Product.Storage;

public class ProductTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = "PRODUCT";
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public double Price { get; set; }
    public int StockLevel { get; set; }
}
