using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.OrderEntities;

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

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public PaymentMethod PaymentMethod { get; set; }

    public string? TransactionId { get; set; }



    [ForeignKey(nameof(OrderId))]
    public int OrderId { get; set; }
}
