using DataAccessLibrary.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;
public class ShoppingCartProduct
{
    [Key]
    public int OrderProductId { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [Required]
    public int ItemQuantity { get; set; }



    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey(nameof(ShoppingCartId))]
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
}
