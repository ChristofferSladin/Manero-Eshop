using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public class Card
{
    [Key]
    public int CardId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string CardNumber { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string CardHolderName { get; set; } = null!;

    [Required]
    public DateTime ExpirationDate { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string CardType { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string IssuerBank { get; set; } = null!;


    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
