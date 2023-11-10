using ServiceLibrary.Models;
using System.Security.Policy;

namespace ManeroWebApp.Models
{
    public class FavoriteProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductNumber { get; set; } = null!;
        public string? UserId { get; set; }
        public int ShoppingCartId { get; set; }
        public string? ImgUrl { get; set; }
        public string? Name { get; set; }
        public decimal PriceWithTax { get; set; }
        public decimal PriceWithoutTax { get; set; }
        public decimal? SalePricePercentage { get; set; }
        public decimal? Rating { get; set; }
        public bool? IsOnSale { get; set; }
        public decimal? SalePrice => (PriceWithoutTax * SalePricePercentage);

        public static implicit operator FavoriteProductViewModel(FavoriteProduct favoriteProduct)
        {
            return new FavoriteProductViewModel
            {
                ProductId = favoriteProduct.ProductId,
                ProductNumber = favoriteProduct.ProductNumber,
                UserId = favoriteProduct.UserId,
                ImgUrl = favoriteProduct.ImgUrl,
                Name = favoriteProduct.Name,
                PriceWithoutTax = favoriteProduct.PriceWithoutTax,
                PriceWithTax = favoriteProduct.PriceWithTax,
                SalePricePercentage = favoriteProduct.SalePricePercentage,
                Rating = favoriteProduct.Rating,
                IsOnSale = favoriteProduct.IsOnSale,
                ShoppingCartId = favoriteProduct.ShoppingCartId,
            };
        }
        public static implicit operator FavoriteProductViewModel(Product product)
        {
            return new FavoriteProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber!,
                ImgUrl = product.ImageUrl,
                Name = product.ProductName,
                PriceWithoutTax = product.PriceExcTax,
                PriceWithTax = product.PriceIncTax,
                IsOnSale = product.IsOnSale,
                Rating = product.Rating,
            };
        }
    }
}
