using CloudOrder.Contracts.DTOs;
using CloudOrder.Product.Domain;
using CloudOrder.Product.Mappers;
using CloudOrder.Product.Storage;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            StockLevel = request.StockLevel
        };

        await _repository.CreateAsync(product);

        var dto = ProductMapper.ToDto(product);
        return CreatedAtAction(nameof(GetById), new { productId = product.Id }, dto);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetById(string productId)
    {
        var product = await _repository.GetByIdAsync(productId);

        if (product is null)
            return NotFound();

        return Ok(ProductMapper.ToDto(product));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _repository.GetAllAsync();

        var result = products
            .Select(ProductMapper.ToDto)
            .ToList();

        return Ok(result);
    }

    [HttpPatch("{productId}/stock")]
    public async Task<IActionResult> UpdateStock(
        string productId,
        [FromBody] UpdateStockRequest request)
    {
        var existing = await _repository.GetByIdAsync(productId);

        if (existing is null)
            return NotFound();

        await _repository.UpdateStockAsync(productId, request.StockLevel);

        return NoContent();
    }

}
