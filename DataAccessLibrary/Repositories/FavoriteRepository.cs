using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DataAccessLibrary.Repositories;

public class FavoriteRepository : Repository<Favorite>
{
    public FavoriteRepository(ManeroDbContext context) : base(context)
    {
    }
}
