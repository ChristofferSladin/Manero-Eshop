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

public enum PaymentMethod
{
    Default,
    Card,
    Receipt,
    PayPal,
    Klarna,
    Swish,
    ApplePay
}

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(12)")]
    public string? OrderNumber { get; private set; }

    public void GenerateProductNumber()
    {
        OrderNumber = OrderId.ToString("D12");
    }

    public string? PromoCode { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public OrderStatus OrderStatus { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public PaymentMethod PaymentMethod { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public DateTime Created { get; set; }


    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;


    [Required]
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = null!;
}
