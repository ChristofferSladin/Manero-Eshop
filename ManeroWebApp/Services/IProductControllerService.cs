using ManeroWebApp.Models;
using System.Security.Claims;

namespace ManeroWebApp.Services
{
    public interface IProductControllerService
    {
        Task<List<ShoppingCartViewModel>> GetShoppingForUserCartAsync(string user);
        Task<List<ShoppingCartViewModel>> GetShoppingForGuestCartAsync(string shoppingCartCookie);
        Task AddProductToShoppingCartForUserAsync(string user, int itemQuantity, string productNumber);
        public void AddProductToShoppingCartForGuest(HttpResponse response, string? existingShoppingCartCookie, int itemQuantity, string productNumber);
        Task<List<ProductViewModel>> GetProductsWithReviewsAsync();
        Task<ProductViewModel> GetProductAsync(string productNumber);
        Task<RatingViewModel> GetReviewDataAsync(string productName);
        Task<List<SizeViewModel>> GetProductSizesAsync(string productName, string productNumber);
        Task<List<ColorViewModel>> GetProductColorsAsync(string productName, string size);
        Task<List<ReviewViewModel>> GetReviewsForProductAsync(string productName, int? take);
    }
}
