using Azure.Data.Tables;
using CloudOrder.Contracts;
using CloudOrder.Order.Mappers;

namespace CloudOrder.Order.Storage;

public class TableOrderRepository : IOrderRepository
{
    private readonly TableClient _tableClient;

    public TableOrderRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
    }

    public async Task<Domain.Order?> GetByIdAsync(string customerId, string orderId)
    {
        var response = await _tableClient.GetEntityIfExistsAsync<OrderTableEntity>(
            partitionKey: customerId,
            rowKey: orderId);

        return response.HasValue
            ? OrderMapper.ToEntity(response.Value)
            : null;
    }

    public async Task<IReadOnlyList<Domain.Order>> GetByCustomerIdAsync(string customerId)
    {
        var allEntities = _tableClient.QueryAsync<OrderTableEntity>(ent => ent.PartitionKey == customerId);

        var orders = new List<Domain.Order>();
        await foreach (var entity in allEntities)
        {
            orders.Add(OrderMapper.ToEntity(entity));
        }
        return orders;
    }

    public async Task CreateAsync(Domain.Order order)
    {
        var entity = OrderMapper.ToTableEntity(order);
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task UpdateStatusAsync(string customerId, string orderId, OrderStatus status)
    {
        var entity = await _tableClient.GetEntityAsync<OrderTableEntity>(customerId, orderId);
        entity.Value.Status = status.ToString();

        await _tableClient.UpdateEntityAsync(entity.Value, entity.Value.ETag);
    }

    public async Task<IReadOnlyList<Domain.Order>> GetAllAsync()
    {
        var allEntities = _tableClient.QueryAsync<OrderTableEntity>();

        var orders = new List<Domain.Order>();
        await foreach (var entity in allEntities)
        {
            orders.Add(OrderMapper.ToEntity(entity));
        }
        return orders;
    }
}
