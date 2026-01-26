namespace CloudOrder.Contracts.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string ImageUrl,
    decimal Price,
    int StockLevel
);

public record CreateProductRequest(
    string Name,
    string ImageUrl,
    decimal Price,
    int StockLevel
);

public record UpdateStockRequest(
    int StockLevel
);
