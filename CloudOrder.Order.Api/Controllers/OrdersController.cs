using CloudOrder.Contracts;
using CloudOrder.Contracts.DTOs;
using CloudOrder.Order.Domain;
using CloudOrder.Order.Mappers;
using CloudOrder.Order.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CloudOrder.Order.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repository;
    private readonly HttpClient _productClient;

    public OrdersController(IOrderRepository repository, IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _productClient = httpClientFactory.CreateClient("ProductApi");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        // Fetch product prices and build order items
        foreach (var item in request.Items)
        {
            var response = await _productClient.GetAsync($"api/products/{item.ProductId}");

            if (!response.IsSuccessStatusCode)
                return BadRequest($"Product {item.ProductId} not found");

            var product = await response.Content.ReadFromJsonAsync<ProductDto>();

            if (product is null)
                return BadRequest($"Product {item.ProductId} not found");

            var orderItem = new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = product.Price
            };

            orderItems.Add(orderItem);
            totalAmount += orderItem.Price * orderItem.Quantity;
        }

        var order = new Domain.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Items = orderItems,
            Amount = totalAmount,
            Status = OrderStatus.Created
        };

        await _repository.CreateAsync(order);

        var dto = OrderMapper.ToDto(order);
        return CreatedAtAction(nameof(GetById), new { orderId = order.Id }, dto);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetById(string orderId)
    {
        // Search across all partitions for the order
        var orders = await _repository.GetAllAsync();
        var order = orders.FirstOrDefault(o => o.Id.ToString() == orderId);

        if (order is null)
            return NotFound();

        return Ok(OrderMapper.ToDto(order));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? customerId = null)
    {
        IReadOnlyList<Domain.Order> orders;

        if (!string.IsNullOrEmpty(customerId))
        {
            orders = await _repository.GetByCustomerIdAsync(customerId);
        }
        else
        {
            orders = await _repository.GetAllAsync();
        }

        var result = orders
            .Select(o => OrderMapper.ToDto(o))
            .ToList();

        return Ok(result);
    }

    [HttpPatch("{orderId}/status")]
    public async Task<IActionResult> UpdateStatus(
        string orderId,
        [FromBody] UpdateOrderStatusRequest request)
    {
        // Find the order first
        var orders = await _repository.GetAllAsync();
        var order = orders.FirstOrDefault(o => o.Id.ToString() == orderId);

        if (order is null)
            return NotFound();

        await _repository.UpdateStatusAsync(order.CustomerId.ToString(), orderId, request.Status);

        return NoContent();
    }
}
