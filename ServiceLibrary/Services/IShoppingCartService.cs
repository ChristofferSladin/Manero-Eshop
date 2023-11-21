using ServiceLibrary.Models;

namespace ServiceLibrary.Services;
public interface IShoppingCartService
{
    Task<bool> IncrementProductInShoppingCartAsync(Increment increment, string productNumber);
    Task<bool> RemoveProductFromShoppingCartAsync(string productNumber);
    Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync();
    Task<bool> AddProductToShoppingCartAsync(int itemQuantity, string productNumber);
}
