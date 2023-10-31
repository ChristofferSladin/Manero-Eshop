using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLibrary.Entities.ProductEntities;

namespace DataAccessLibrary.Entities.UserEntities;

public enum OrderStatus
{
    Default,
    Completed,
    Canceled,
    InProcess
}



public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Column(TypeName = "nvarchar(12)")]
    public string? OrderNumber { get; private set; }

    public void GenerateOrderNumber()
    {
        OrderNumber = OrderId.ToString("D12");
    }


    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public OrderStatus OrderStatus { get; set; }

    [Required]
    public decimal TotalPriceExcTax { get; set; } 

    [Required]
    public decimal TotalPriceIncTax { get; set; }

    public decimal VatTax { get; set; }

    [Required]
    public decimal TaxPercentage { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime PaymentDate { get; set; }



    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(PaymentId))]
    public int PaymentId { get; set; }
    [Required]
    public Payment Payment { get; set; } = null!;

    [Required]
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = null!;

    public PromoCode? PromoCode { get; set; }

}
