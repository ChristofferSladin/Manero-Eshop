using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Entities.UserEntities;

namespace DataAccessLibrary.Entities.ProductEntities;

public class OrderProduct
{
    [Key]
    public int OrderProductId { get; set; }

    [Required]
    public int ItemQuantity { get; set; }



    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}