using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLibrary.Entities.UserEntities;

namespace DataAccessLibrary.Entities.ProductEntities;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Column(TypeName = "nvarchar(12)")]
    public string? ProductNumber { get; private set; }

    public void GenerateProductNumber()
    {
        ProductNumber = ProductId.ToString("D12");
    }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "nvarchar(500)")]
    public string? Description { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? Category { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? Type { get; set; }

    [Column(TypeName = "nvarchar(20)")]
    public string? Size { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    [Required]
    public decimal Price { get; set; }

    public decimal SalePrice { get; set; }

    public bool IsOnSale { get; set; }

    public bool IsFeatured { get; set; }

    public decimal Rating { get; set; }

    [Column(TypeName = "nvarchar(300)")]
    public string? ImageUrl { get; set; }


    public virtual ICollection<OrderProduct>? OrdersProducts { get; set; }
    public virtual ICollection<ProductColor>? ProductColors { get; set; }
    public virtual ICollection<ShoppingCartProduct>? ShoppingCartProducts { get; set; }
    public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
}
