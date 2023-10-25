using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLibrary.Entities.ProductEntities;

namespace DataAccessLibrary.Entities.UserEntities;

public class ShoppingCart
{
    [Key] 
    public int ShoppingCartId { get; set; }

    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;


    public virtual ICollection<ShoppingCartProduct>? ShoppingCartProducts { get; set; }
}
