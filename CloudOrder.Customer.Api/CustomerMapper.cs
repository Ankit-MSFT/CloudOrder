using CloudOrder.Contracts.DTOs;
using CloudOrder.Customer.Domain;
using CloudOrder.Customer.TableEntities;

namespace CloudOrder.Customer.Mappers;

public static class CustomerMapper
{
    public static Domain.Customer ToEntity(CustomerTableEntity e)
    {
        return new Domain.Customer
        {
            Id = Guid.Parse(e.RowKey),
            Email = e.Email,
            Address = new Address
            {
                Line1 = e.AddressLine1,
                City = e.City,
                State = e.State,
                Country = e.Country,
                PostalCode = e.PostalCode
            }
        };
    }

    public static CustomerTableEntity ToTableEntity(Domain.Customer customer)
    {
        return new CustomerTableEntity
        {
            PartitionKey = "CUSTOMER",
            RowKey = customer.Id.ToString(),
            Email = customer.Email,

            AddressLine1 = customer.Address.Line1,
            City = customer.Address.City,
            State = customer.Address.State,
            Country = customer.Address.Country,
            PostalCode = customer.Address.PostalCode
        };
    }

    public static Domain.Customer ToEntity(CreateCustomerRequest req)
    {
        return new Domain.Customer
        {
            Id = Guid.NewGuid(),
            Email = req.Email.Trim(),
            Address = new Address
            {
                Line1 = req.Address.Line1.Trim(),
                City = req.Address.City.Trim(),
                State = req.Address.State.Trim(),
                Country = req.Address.Country.Trim(),
                PostalCode = req.Address.PostalCode.Trim()
            }
        };
    }

    public static CustomerDto ToDto(Domain.Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.Email,
            new AddressDto(
                customer.Address.Line1,
                customer.Address.City,
                customer.Address.State,
                customer.Address.Country,
                customer.Address.PostalCode
            )
        );
    }
}
