using ServiceLibrary.Models;
using System.Security.Claims;
using UserAPI.Models;

namespace ServiceLibrary.Services;

public interface IUserService
{
    Task<IEnumerable<FavoriteProduct>> GetAllFavoriteProductsForUserAsync(string userId);
    Task<ShoppingCart> GetShoppingCartByUser(string userId);
    Task<ShoppingCartProduct> CreateShoppingCartProductEntry(int productId, int shoppingCartId);
    Task<FavoriteProduct> AddProductToWishList(int productId, string userId);
    Task<UserProfile> GetUserProfileAsync(string id);
    Task<bool> RemoveProductFromWishListAsync(int productId, string userId);
    Task<RefreshModel> GetUserToken(string email, string password);
}
