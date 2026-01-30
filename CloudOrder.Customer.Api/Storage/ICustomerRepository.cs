namespace CloudOrder.Customer.Storage;

public interface ICustomerRepository
{
    Task<Domain.Customer?> GetByIdAsync(string customerId);
    Task<IReadOnlyList<Domain.Customer>> GetAllAsync();
    Task CreateAsync(Domain.Customer customer);
    Task UpdateAsync(Domain.Customer customer);
}
