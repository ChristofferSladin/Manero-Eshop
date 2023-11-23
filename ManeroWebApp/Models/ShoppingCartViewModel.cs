using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public string? ProductNumber { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal PriceExcTax { get; set; }
        public decimal PriceIncTax { get; set; }
        public decimal SalePricePercentage { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOnSale { get; set; }
        public int ItemQuantity { get; set; }

        public static implicit operator ShoppingCartViewModel(Product product)
        {
            return new ShoppingCartViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.ProductName,
                Category = product.Category,
                Size = product.Size,
                Color = product.Color,
                PriceExcTax = product.PriceExcTax,
                PriceIncTax = product.PriceIncTax,
                SalePricePercentage = product.SalePricePercentage,
                ImageUrl = product.ImageUrl,
                IsOnSale = product.IsOnSale
            };
        }
    }
}
