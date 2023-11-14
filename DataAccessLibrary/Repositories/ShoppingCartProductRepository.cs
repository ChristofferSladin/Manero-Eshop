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

    public async Task<bool> AddProductAndQuantityToCart(string user, int quantity, string productNumber)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user);
        if (userExists == null) return false;
        var shoppingCartExists = await _context.ShoppingCarts.FirstOrDefaultAsync(o => o.Id == userExists.Id);
        if (shoppingCartExists == null)
        {
            var shoppingCart = new ShoppingCart
            {
                Id = userExists.Id
            };
            await _context.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();
        }

        if (shoppingCartExists == null) return false;
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductNumber == productNumber);
        if (product == null) return false;
        var existingCartItem = await _context.ShoppingCartProducts.FirstOrDefaultAsync(c => c.ShoppingCartId == shoppingCartExists.ShoppingCartId && c.ProductId == product.ProductId);
        if (existingCartItem != null)
        {
            existingCartItem.ItemQuantity += quantity;
            _context.Update(existingCartItem);
        }
        else
        {

            var shoppingCartProduct = new ShoppingCartProduct
            {
                ItemQuantity = quantity,
                Product = product,
                TotalPriceExcTax = product.PriceExcTax * quantity,
                TotalPriceIncTax = product.PriceIncTax * quantity,
                ShoppingCartId = shoppingCartExists.ShoppingCartId
            };
            await _context.AddAsync(shoppingCartProduct);
        }
        await _context.SaveChangesAsync();
        return true;
    }
}
