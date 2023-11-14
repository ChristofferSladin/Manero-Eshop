using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.OrderEntities;

public class OrderProduct
{
    [Key]
    public int OrderProductId { get; set; }

    [Required]
    public int ItemQuantity { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(12)")]
    public string ProductNumber { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProductName { get; set; } = null!;

    [Required]
    public decimal PriceExcTax { get; set; }

    [Required]
    public decimal PriceIncTax { get; set; }

    public decimal SalePricePercentage { get; set; }


    [ForeignKey(nameof(OrderId))]
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}