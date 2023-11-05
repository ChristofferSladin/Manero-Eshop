using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;

namespace DataAccessLibrary.Repositories;

public class ShoppingCartRepository : Repository<ShoppingCart>
{
    public ShoppingCartRepository(ManeroDbContext context) : base(context)
    {
    }
}
