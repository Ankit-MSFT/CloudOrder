
using Azure.Data.Tables;
using CloudOrder.Product.Mappers;

namespace CloudOrder.Product.Storage;

public class TableProductRepository : IProductRepository
{
    private readonly TableClient _tableClient;

    public TableProductRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
    }

    public async Task<Domain.Product?> GetByIdAsync(string productId)
    {
        var response = await _tableClient.GetEntityIfExistsAsync<ProductTableEntity>(
            partitionKey: "PRODUCT",
            rowKey: productId);

        return response.HasValue
            ? ProductMapper.ToEntity(response.Value)
            : null;
    }

    public async Task CreateAsync(Domain.Product product)
    {
        var entity = ProductMapper.ToTableEntity(product);
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task UpdateStockAsync(string productId, int stockLevel)
    {
        var entity = await _tableClient.GetEntityAsync<ProductTableEntity>("PRODUCT", productId);
        entity.Value.StockLevel = stockLevel;

        await _tableClient.UpdateEntityAsync(entity.Value, entity.Value.ETag);
    }

    public async Task<IReadOnlyList<Domain.Product>> GetAllAsync()
    {
        var allEntities = _tableClient.QueryAsync<ProductTableEntity>(ent => ent.PartitionKey == "PRODUCT");

        var products = new List<Domain.Product>();
        await foreach (var entity in allEntities)
        {
            products.Add(ProductMapper.ToEntity(entity));
        }
        return products;
    }
}
