using Azure;
using Azure.Data.Tables;

namespace CloudOrder.Order.Storage;

public class OrderTableEntity : ITableEntity
{
    public string PartitionKey { get; set; } = default!; // customerId
    public string RowKey { get; set; } = default!;       // orderId
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public double Amount { get; set; }
    public string Status { get; set; } = default!;
    public string ItemsJson { get; set; } = default!;
}
