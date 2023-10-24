
using DataAccessLibrary.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public class ShoppingCart
{
    [Key] 
    public int ShoppingCartId { get; set; }
    public ICollection<Product>? Products { get; set; }
    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
