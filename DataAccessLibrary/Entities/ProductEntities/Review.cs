
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.ProductEntities;

public class Review
{
    [Key]
    public int UserReviewId { get; set; }
    [Required]
    public int Rating { get; set; }
    [Required]
    public DateTime Created { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string Content { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string Title { get; set; } = null!;
    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(ProductId))]
    public int ProductId { get; set; }
}
