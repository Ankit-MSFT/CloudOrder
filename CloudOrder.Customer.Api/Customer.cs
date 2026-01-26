namespace CloudOrder.Customer.Domain;

public class Customer
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Address Address { get; set; } = new Address();
}