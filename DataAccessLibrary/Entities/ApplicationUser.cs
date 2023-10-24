using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Entities.ProductEntities;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<Adress>? Adresses { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<Card>? Cards { get; set; }
    public virtual ICollection<ShoppingCart>? ShoppingCarts { get; set; }
    public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
}
