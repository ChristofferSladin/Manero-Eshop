using System.ComponentModel.DataAnnotations;

namespace ManeroWebApp.Models;

public class AddPromoCodeViewModel
{
    [Required]
    public string Voucher { get; set; } = null!;
}
