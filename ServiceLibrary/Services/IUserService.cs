using ServiceLibrary.Models;

namespace ServiceLibrary.Services;

public interface IUserService
{
    Task<IEnumerable<FavoriteProduct>> GetAllFavoriteProductsForUserAsync(string userId);
    Task<DataAccessLibrary.Entities.UserEntities.ShoppingCart> GetShoppingCartByUser(string userId);
    Task<DataAccessLibrary.Entities.UserEntities.ShoppingCartProduct> CreateShoppingCartProductEntry(int productId, int shoppingCartId);
    Task<DataAccessLibrary.Entities.UserEntities.FavoriteProduct> AddProductToWishList(int productId, string userId);
}
