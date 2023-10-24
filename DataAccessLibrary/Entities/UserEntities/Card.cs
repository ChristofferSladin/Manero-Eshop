using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public class Card
{
    [Key]
    public int CardId { get; set; }
    [Required]
    public string CardNumber { get; set; } = null!;
    [Required]
    public string CardHolderName { get; set; } = null!;
    [Required]
    public DateTime ExpirationDate { get; set; } 
    [Required]
    public int SecurityCode { get; set; }
    [Required]
    public string CardType { get; set; } = null!;
    [Required]
    public string IssuerBank { get; set; } = null!;

    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
