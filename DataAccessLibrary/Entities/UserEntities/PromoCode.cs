using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public class PromoCode
{
    [Key]
    public int PromoCodeId { get; set; }

    [Required] 
    public string PromoCodeName { get; set; } = null!;
    public decimal PromoCodePercentage { get; set; }
    public decimal? PromoCodeAmount { get; set; }
    public bool PromoCodeIsUsed { get; set; }
    public DateTime PromoCodeValidity { get; set; }
    public string PromoCodeText { get; set; } = null!;
    public string? PromoCodeImgUrl { get; set; }
    public virtual ICollection<UserPromoCode>? UserPromoCodes { get; set; }
}