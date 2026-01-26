using CloudOrder.Contracts.DTOs;
using CloudOrder.Product.Storage;

namespace CloudOrder.Product.Mappers;

public static class ProductMapper
{
    public static Domain.Product ToEntity(ProductTableEntity e)
    {
        return new Domain.Product
        {
            Id = Guid.Parse(e.RowKey),
            Name = e.Name,
            ImageUrl = e.ImageUrl,
            Price = Convert.ToDecimal(e.Price),
            StockLevel = e.StockLevel
        };
    }

    public static ProductTableEntity ToTableEntity(Domain.Product product)
    {
        return new ProductTableEntity
        {
            PartitionKey = "PRODUCT",
            RowKey = product.Id.ToString(),
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Price = Convert.ToDouble(product.Price),
            StockLevel = product.StockLevel
        };
    }

    public static Domain.Product ToEntity(CreateProductRequest req)
    {
        return new Domain.Product
        {
            Id = Guid.NewGuid(),
            Name = req.Name.Trim(),
            ImageUrl = req.ImageUrl.Trim(),
            Price = req.Price,
            StockLevel = req.StockLevel
        };
    }

    public static ProductDto ToDto(Domain.Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Price,
            product.StockLevel
        );
    }
}
