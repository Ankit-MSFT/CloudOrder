using CloudOrder.Contracts;

namespace CloudOrder.Order.Storage;

public interface IOrderRepository
{
    Task<Domain.Order?> GetByIdAsync(string customerId, string orderId);
    Task<IReadOnlyList<Domain.Order>> GetByCustomerIdAsync(string customerId);
    Task<IReadOnlyList<Domain.Order>> GetAllAsync();
    Task CreateAsync(Domain.Order order);
    Task UpdateStatusAsync(string customerId, string orderId, OrderStatus status);
}
