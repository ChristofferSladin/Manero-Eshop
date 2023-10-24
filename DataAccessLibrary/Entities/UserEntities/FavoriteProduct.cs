
using DataAccessLibrary.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Entities.UserEntities;

public class FavoriteProduct
{
    [Key]
    public int FavoriteProductId { get; set; }
    public ICollection<Product>? Products { get; set; }
    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
