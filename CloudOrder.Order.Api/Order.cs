using CloudOrder.Contracts;

namespace CloudOrder.Order.Domain;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal Amount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
}