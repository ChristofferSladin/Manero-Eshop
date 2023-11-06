using ServiceLibrary.Models;
using System.Security.Policy;

namespace ManeroWebApp.Models
{
    public class FavoriteProductViewModel
    {
        public int ProductId { get; set; }
        public string UserId {  get; set; }
        public int ShoppingCartId { get; set; }
        public string? ImgUrl { get; set; }
        public string? Name {  get; set; }
        public decimal PriceWithTax { get; set; }
        public decimal PriceWithoutTax { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? Rating { get; set; }
        public bool? IsOnSale { get; set; }

        public static implicit operator FavoriteProductViewModel(FavoriteProduct favoriteProduct)
        {
            return new FavoriteProductViewModel
            {
                ProductId = favoriteProduct.ProductId,
                UserId = favoriteProduct.UserId,
                ImgUrl = favoriteProduct.ImgUrl,
                Name = favoriteProduct.Name,
                PriceWithoutTax = favoriteProduct.PriceWithoutTax,
                PriceWithTax = favoriteProduct.PriceWithTax,
                SalePrice = favoriteProduct.SalePrice,
                Rating = favoriteProduct.Rating,
                IsOnSale = favoriteProduct.IsOnSale,
                ShoppingCartId = favoriteProduct.ShoppingCartId,
            };
        }
    }
}
