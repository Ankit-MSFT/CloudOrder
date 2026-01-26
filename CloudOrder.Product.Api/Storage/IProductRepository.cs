namespace CloudOrder.Product.Storage;

public interface IProductRepository
{
    Task<Domain.Product?> GetByIdAsync(string productId);
    Task<IReadOnlyList<Domain.Product>> GetAllAsync();
    Task CreateAsync(Domain.Product product);
    Task UpdateStockAsync(string productId, int stockLevel);
}
