using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Entities.ProductEntities;

namespace ServiceLibrary.Models;

public class Product
{
    public int ProductId { get; set; }
    public string? ProductNumber { get; set; }
    public string ProductName { get; set; } = null!;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public string? Size { get; set; }
    public int QuantityInStock { get; set; }
    public string? Color { get; set; }
    public decimal PriceExcTax { get; set; }
    public decimal PriceIncTax { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsOnSale { get; set; }
    public bool IsFeatured { get; set; }
    public decimal Rating { get; set; }
    public string? ImageUrl { get; set; }
    public List<Review>? Reviews { get; set; }
    public static implicit operator Product(DataAccessLibrary.Entities.ProductEntities.Product productEntity)
    {
        return new Product
        {
            ProductId = productEntity.ProductId,
            ProductNumber = productEntity.ProductNumber,
            ProductName = productEntity.ProductName,
            Description = productEntity.Description,
            Category = productEntity.Category,
            Type = productEntity.Type,
            Size = productEntity.Size,
            QuantityInStock = productEntity.QuantityInStock,
            Color = productEntity.Color,
            PriceExcTax = productEntity.PriceExcTax,
            PriceIncTax = productEntity.PriceIncTax,
            SalePrice = productEntity.SalePrice,
            IsOnSale = productEntity.IsOnSale,
            IsFeatured = productEntity.IsFeatured,
            Rating = productEntity.Rating,
            ImageUrl = productEntity.ImageUrl,
        };
    }
}