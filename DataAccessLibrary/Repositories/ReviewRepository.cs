using DataAccessLibrary.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataAccessLibrary.Entities.ProductEntities;

namespace DataAccessLibrary.Repositories
{
    public class ReviewRepository
    {
        private readonly ManeroDbContext _context;
        public ReviewRepository(ManeroDbContext context)
        {
            _context = context;
        }
        public async Task<List<Review>> GetFilteredReviewsAsync(int skip, int take, Expression<Func<Review, bool>> filterByName = null!, Expression<Func<Review, dynamic>> orderByField = null!, string orderDirection = null!)
        {
            var query = _context.Reviews.AsQueryable();

            if (filterByName != null!)
            {
                query = query.Where(filterByName);
            }

            if (orderByField != null!)
            {
                query = orderDirection switch
                {
                    "asc" => query.OrderBy(orderByField),
                    "desc" => query.OrderByDescending(orderByField),
                    _ => query
                };
            }

            if (take == 0)
            {
                return await query.ToListAsync();
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }
    }
}
