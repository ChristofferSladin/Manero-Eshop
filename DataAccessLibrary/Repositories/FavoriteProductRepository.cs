using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;

namespace DataAccessLibrary.Repositories;

public class FavoriteProductRepository : Repository<FavoriteProduct>
{
    public FavoriteProductRepository(ManeroDbContext context) : base(context)
    {
    }
}
