using System.Linq.Expressions;
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;
public class ProductRepository
{
    private readonly ManeroDbContext _context;
    public ProductRepository(ManeroDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.FirstOrDefaultAsync(expression) ?? null!;
    }
    public async Task<Product> GetProductWithReviewsAsync(Expression<Func<Product, bool>> expression)
    {
        return await _context.Products.Include(r=>r.Reviews).FirstOrDefaultAsync(expression) ?? null!;
    }
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<List<Product>> GetAllProductsWithReviewsAsync()
    {
        return await _context.Products.Include(r => r.Reviews).ToListAsync();
    }

}
