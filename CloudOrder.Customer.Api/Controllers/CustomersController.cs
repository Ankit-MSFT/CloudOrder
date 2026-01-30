using CloudOrder.Contracts.DTOs;
using CloudOrder.Customer.Domain;
using CloudOrder.Customer.Mappers;
using CloudOrder.Customer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CloudOrder.Customer.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var customer = new Domain.Customer
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Address = new Address
            {
                Line1 = request.Address.Line1,
                City = request.Address.City,
                State = request.Address.State,
                Country = request.Address.Country,
                PostalCode = request.Address.PostalCode
            }
        };

        await _repository.CreateAsync(customer);

        var dto = CustomerMapper.ToDto(customer);
        return CreatedAtAction(nameof(GetById), new { customerId = customer.Id }, dto);
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(string customerId)
    {
        var customer = await _repository.GetByIdAsync(customerId);

        if (customer is null)
            return NotFound();

        return Ok(CustomerMapper.ToDto(customer));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _repository.GetAllAsync();

        var result = customers
            .Select(CustomerMapper.ToDto)
            .ToList();

        return Ok(result);
    }

    [HttpPut("{customerId}")]
    public async Task<IActionResult> Update(
        string customerId,
        [FromBody] CreateCustomerRequest request)
    {
        var existing = await _repository.GetByIdAsync(customerId);

        if (existing is null)
            return NotFound();

        existing.Email = request.Email;
        existing.Address = new Address
        {
            Line1 = request.Address.Line1,
            City = request.Address.City,
            State = request.Address.State,
            Country = request.Address.Country,
            PostalCode = request.Address.PostalCode
        };

        await _repository.UpdateAsync(existing);

        return NoContent();
    }
}
