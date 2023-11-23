using ManeroWebApp.Models;

namespace ManeroWebApp.Services
{
    public interface IProductControllerService
    {
        Task RemoveProductToFavoriteForUserAsync(string productNumber);
        Task AddProductToFavoriteForUserAsync(string productNumber);
        Task<List<FavoriteProductViewModel>> GetFavoritesForUserAsync();
        Task<List<ShoppingCartViewModel>> GetShoppingForUserCartAsync();
        Task<List<ShoppingCartViewModel>> GetShoppingForGuestCartAsync();
        Task AddProductToShoppingCartForUserAsync(int itemQuantity, string productNumber);
        public void AddProductToShoppingCartForGuest(int itemQuantity, string productNumber);
        Task<List<ProductViewModel>> GetProductsWithReviewsAsync();
        Task<ProductViewModel> GetProductAsync(string productNumber);
        Task<RatingViewModel> GetReviewDataAsync(string productName);
        Task<List<SizeViewModel>> GetProductSizesAsync(string productName, string productNumber);
        Task<List<ColorViewModel>> GetProductColorsAsync(string productName, string size);
        Task<List<ReviewViewModel>> GetReviewsForProductAsync(string productName, int? take);
       
    }
}
