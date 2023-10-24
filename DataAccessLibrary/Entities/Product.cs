using DataAccessLibrary.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(12)")]
    public int ProductNumber {  get; set; }
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProductName { get; set; } = null!;
    [Column(TypeName = "nvarchar(500)")]
    public string? Description { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string? Category { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    public string? Type { get; set; }
    public virtual ICollection<ProductColor>? ProductColors { get; set; }
    public string? Size { get; set; }
    [Required]
    public decimal Price { get; set; }
    public decimal SalePrice { get; set; }
    public bool IsOnSale { get; set; }
    public bool IsFeatured { get; set; }
    public decimal Rating { get; set; }
}
