using Azure.Data.Tables;
using CloudOrder.Customer.Mappers;

namespace CloudOrder.Customer.Storage;

public class TableCustomerRepository : ICustomerRepository
{
    private readonly TableClient _tableClient;

    public TableCustomerRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
    }

    public async Task<Domain.Customer?> GetByIdAsync(string customerId)
    {
        var response = await _tableClient.GetEntityIfExistsAsync<CustomerTableEntity>(
            partitionKey: "CUSTOMER",
            rowKey: customerId);

        return response.HasValue
            ? CustomerMapper.ToEntity(response.Value)
            : null;
    }

    public async Task CreateAsync(Domain.Customer customer)
    {
        var entity = CustomerMapper.ToTableEntity(customer);
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task UpdateAsync(Domain.Customer customer)
    {
        var entity = CustomerMapper.ToTableEntity(customer);
        await _tableClient.UpdateEntityAsync(entity, Azure.ETag.All, TableUpdateMode.Replace);
    }

    public async Task<IReadOnlyList<Domain.Customer>> GetAllAsync()
    {
        var allEntities = _tableClient.QueryAsync<CustomerTableEntity>(ent => ent.PartitionKey == "CUSTOMER");

        var customers = new List<Domain.Customer>();
        await foreach (var entity in allEntities)
        {
            customers.Add(CustomerMapper.ToEntity(entity));
        }
        return customers;
    }
}
