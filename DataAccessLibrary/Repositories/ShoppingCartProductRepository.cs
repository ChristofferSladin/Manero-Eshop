using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;

public class ShoppingCartProductRepository : Repository<ShoppingCartProduct>
{
    private readonly ManeroDbContext _context;

    public ShoppingCartProductRepository(ManeroDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<ShoppingCartProduct>> GetShoppingCartProductsAsync(string claim)
    {
        var shoppingCartProducts = await _context.ShoppingCarts
            .Join(
                _context.ShoppingCartProducts,
                sc => sc.ShoppingCartId,
                scp => scp.ShoppingCartId,
                (sc, scp) => new { ShoppingCart = sc, ShoppingCartProduct = scp }
            )
            .Join(
                _context.Users,
                result => result.ShoppingCart.Id,
                asp => asp.Id,
                (result, asp) => new { result.ShoppingCart, result.ShoppingCartProduct, AspNetUser = asp }
            )
            .Where(joinedResult => joinedResult.AspNetUser.Email == claim)
            .Select(joinedResult => joinedResult.ShoppingCartProduct)
            .ToListAsync();

        return shoppingCartProducts;

    }
}
