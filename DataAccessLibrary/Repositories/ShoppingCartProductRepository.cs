using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;

namespace DataAccessLibrary.Repositories;

public class ShoppingCartProductRepository : Repository<ShoppingCartProduct>
{
    public ShoppingCartProductRepository(ManeroDbContext context) : base(context)
    {
    }

}
