using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.ProductEntities;

public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Required] 
    public string ProductName { get; set; } = null!;

    [Required]
    public int Rating { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(500)")]
    public string Content { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string Title { get; set; } = null!;
   

    [Required]
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }


    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
