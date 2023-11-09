using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Entities.ProductEntities;

namespace ProductAPI.Dtos
{
    public class ProductDto
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

        //public virtual ICollection<OrderProduct>? OrdersProducts { get; set; }
        //public virtual ICollection<ShoppingCartProduct>? ShoppingCartProducts { get; set; }
        //public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
        public virtual ICollection<ReviewDto>? Reviews { get; set; }
    }
}
