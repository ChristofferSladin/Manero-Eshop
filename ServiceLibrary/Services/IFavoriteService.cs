using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IFavoriteService
    {
        Task<bool> RemoveProductToFavoritesAsync(string productNumber);
        Task<bool> AddProductToFavoritesAsync(string productNumber);
        Task<List<FavoriteProduct>> GetUserFavoriteProductsAsync();
    }
}