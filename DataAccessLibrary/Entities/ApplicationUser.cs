using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<Address>? Addresses { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<Card>? Cards { get; set; }
    public virtual ShoppingCart? ShoppingCart { get; set; }
    public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
}
