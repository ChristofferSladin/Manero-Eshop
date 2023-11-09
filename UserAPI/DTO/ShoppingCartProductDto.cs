using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.DTO;

public class ShoppingCartProductDto
{
    public int ShoppingCartProductId { get; set; }
    public decimal TotalPriceIncTax { get; set; }
    public decimal TotalPriceExcTax { get; set; }
    public int ItemQuantity { get; set; }
    public int ProductId { get; set; }
    public int ShoppingCartId { get; set; }
    
    public static implicit operator ShoppingCartProductDto(ShoppingCartProduct shoppingCartProduct)
    {
        return new ShoppingCartProductDto
        {
            ShoppingCartProductId = shoppingCartProduct.ShoppingCartProductId,
            TotalPriceExcTax = shoppingCartProduct.TotalPriceExcTax,
            TotalPriceIncTax = shoppingCartProduct.TotalPriceIncTax,
            ItemQuantity = shoppingCartProduct.ItemQuantity,
            ProductId = shoppingCartProduct.ProductId,
            ShoppingCartId = shoppingCartProduct.ShoppingCartId,
        };
    }
}
