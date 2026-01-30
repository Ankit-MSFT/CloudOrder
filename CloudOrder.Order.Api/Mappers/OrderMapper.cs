using System.Text.Json;
using CloudOrder.Contracts;
using CloudOrder.Contracts.DTOs;
using CloudOrder.Order.Domain;
using CloudOrder.Order.Storage;

namespace CloudOrder.Order.Mappers;

public static class OrderMapper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public static Domain.Order ToEntity(OrderTableEntity e)
    {
        var items = JsonSerializer.Deserialize<List<OrderItem>>(e.ItemsJson, JsonOptions) ?? new List<OrderItem>();

        return new Domain.Order
        {
            Id = Guid.Parse(e.RowKey),
            CustomerId = Guid.Parse(e.PartitionKey),
            Items = items,
            Amount = Convert.ToDecimal(e.Amount),
            Status = Enum.Parse<OrderStatus>(e.Status, ignoreCase: true)
        };
    }

    public static OrderTableEntity ToTableEntity(Domain.Order order)
    {
        return new OrderTableEntity
        {
            PartitionKey = order.CustomerId.ToString(),
            RowKey = order.Id.ToString(),
            Amount = Convert.ToDouble(order.Amount),
            Status = order.Status.ToString(),
            ItemsJson = JsonSerializer.Serialize(order.Items, JsonOptions)
        };
    }

    public static OrderDto ToDto(Domain.Order order, CustomerDto? customer = null)
    {
        var dtoItems = order.Items
            .Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.Price))
            .ToList();

        var customerDto = customer ?? new CustomerDto(order.CustomerId, "", null);
        return new OrderDto(
            order.Id,
            customerDto,
            dtoItems,
            order.Amount,
            order.Status.ToString()
        );
    }
}
