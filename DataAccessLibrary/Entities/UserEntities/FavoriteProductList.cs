using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLibrary.Entities.ProductEntities;

namespace DataAccessLibrary.Entities.UserEntities;

public class FavoriteProductList
{
    [Key]
    public int FavoriteProductId { get; set; }


    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;


    public virtual ICollection<Product>? Products { get; set; }
}
