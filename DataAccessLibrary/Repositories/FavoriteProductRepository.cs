using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Repositories;

public class FavoriteProductRepository : Repository<FavoriteProduct>
{
    private readonly ManeroDbContext _context;
    public FavoriteProductRepository(ManeroDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<FavoriteProduct>> GetFavoriteProductsAsync(string claim)
    {
        var favoriteProducts = await _context.Favorite.Include(u => u.FavoriteProducts)
            .Where(s => s.Id == claim).SelectMany(s => s.FavoriteProducts!).ToListAsync();

        foreach (var favProduct in favoriteProducts)
            favProduct.Product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == favProduct.ProductId);

        return favoriteProducts;
    }
    public async Task<bool> AddProductToFavorites(string user, string productNumber)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == user);
        if (userExists == null) return false;
        var favorite = await _context.Favorite.FirstOrDefaultAsync(o => o.Id == userExists.Id);
        if (favorite == null)
        {
            favorite = new Favorite
            {
                Id = userExists.Id
            };
            await _context.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        if (favorite == null!) return false;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductNumber == productNumber);
        if (product == null) return false;
        var existingFavoriteItem = await _context.FavoriteProducts.FirstOrDefaultAsync(c => c.FavoriteId == favorite.FavoriteId && c.ProductId == product.ProductId);
        if (existingFavoriteItem == null)
        {
            var favoriteProduct = new FavoriteProduct
            {
                ProductId = product.ProductId,
                FavoriteId = favorite.FavoriteId
            };
            await _context.AddAsync(favoriteProduct);
        }
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveProductToFavorites(string user, string productNumber)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Id == user);
        if (userExists == null) return false;
        var favorite = await _context.Favorite.FirstOrDefaultAsync(o => o.Id == userExists.Id);
        if (favorite == null!) return false;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductNumber == productNumber);
        if (product == null) return false;
        var existingFavoriteItem = await _context.FavoriteProducts.FirstOrDefaultAsync(c => c.FavoriteId == favorite.FavoriteId && c.ProductId == product.ProductId);
        if (existingFavoriteItem == null) return true;
        _context.Remove(existingFavoriteItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
