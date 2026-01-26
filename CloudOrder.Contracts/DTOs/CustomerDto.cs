namespace CloudOrder.Contracts.DTOs;

public record AddressDto(
    string Line1,
    string City,
    string State,
    string Country,
    string PostalCode
);

public record CustomerDto(
    Guid Id,
    string Email,
    AddressDto Address
);

public record CreateCustomerRequest(
    string Email,
    AddressDto Address
);

