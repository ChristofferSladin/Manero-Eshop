
using DataAccessLibrary.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public enum OrderStatus
{
    Default,
    Completed,
    Canceled,
    InProccess
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
    public int OrderNumber { get; set; }
    [Required]
    public ICollection<Product> Products { get; set; } = null!;
    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
    [Required]
    public OrderStatus OrderStatus { get; set; }
    public string? PromoCode { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public PaymentMethod PaymentMethod { get; set; }
    [Required]
    public decimal TotalAmount { get; set; }
}
