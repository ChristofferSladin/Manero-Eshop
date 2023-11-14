using ManeroWebApp.Models;
using System.Security.Claims;

namespace ManeroWebApp.Services
{
    public interface IShoppingCartControllerService
    {
        Task<List<ShoppingCartViewModel>> GetShoppingForUserCart(string user);
        Task<List<ShoppingCartViewModel>> GetShoppingForGuestCart(string shoppingCartCookie);
        Task AddProductToShoppingCartForUser(string user, int itemQuantity, string productNumber);

        public void AddProductToShoppingCartForGuest(HttpResponse response, string? existingShoppingCartCookie,
            int itemQuantity, string productNumber);
    }
}
