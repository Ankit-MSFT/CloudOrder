namespace CloudOrder.Contracts.DTOs;

public record OrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price
);

public record OrderDto(
    Guid Id,
    CustomerDto Customer,
    List<OrderItemDto> Items,
    decimal Amount,
    string Status
);

public record CreateOrderRequest(
    Guid CustomerId,
    List<CreateOrderItemRequest> Items
);

public record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity
);

public record UpdateOrderStatusRequest(OrderStatus Status);