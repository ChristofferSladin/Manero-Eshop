using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Entities.UserEntities;

public class PromoCode
{
    [Key]
    public int PromoCodeId { get; set; }

    [Required] 
    public string PromoCodeName { get; set; } = null!;

    public decimal PromoCodePercentage { get; set; }

    public decimal PromoCodeAmount { get; set; }
}