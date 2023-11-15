using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class ProductViewModel
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
        public decimal SalePricePercentage { get; set; }
        public bool IsOnSale { get; set; }
        public bool IsFeatured { get; set; }
        public decimal Rating { get; set; }
        public string? ImageUrl { get; set; }
        public List<ReviewViewModel>? Reviews { get; set;}

        public static implicit operator ProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category,
                Type = product.Type,
                Size = product.Size,
                QuantityInStock = product.QuantityInStock,
                Color = product.Color,
                PriceExcTax = product.PriceExcTax,
                PriceIncTax = product.PriceIncTax,
                SalePricePercentage = product.SalePricePercentage,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl,
            };
        }
    }
}
