using ManeroWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace ManeroWebApp.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IShoppingCartService _shoppingCartService;
        public WishListController(IFavoriteService favoriteService, IShoppingCartService shoppingCartService)
        {
            _favoriteService = favoriteService;
            _shoppingCartService = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var favProductsByUser = await _favoriteService.GetUserFavoriteProductsAsync();

                if (favProductsByUser is not null)
                    return View(favProductsByUser.Select(favProduct => (FavoriteProductViewModel)favProduct));
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return View(new List<FavoriteProductViewModel>());
        }
        public async Task<IActionResult> AddProductToShoppingCart(string productNumber)
        {
            try
            {
                var isSuccess = await _shoppingCartService.AddProductToShoppingCartAsync(1, productNumber);
                if (isSuccess)
                {
                    TempData["success"] = $"Product added to the shopping cart.";
                    return RedirectToAction("Index", "WishList");
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = $"Something went wrong.";
            return RedirectToAction("Index", "WishList");
        }
        public async Task<IActionResult> RemoveProductFromWishList(string productNumber)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var isSuccess = await _favoriteService.RemoveProductToFavoritesAsync(productNumber);
                if (isSuccess)
                {
                    TempData["success"] = $"Product deleted from the Wish List.";
                    return RedirectToAction("Index", "WishList");
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = $"Something went wrong.";
            return RedirectToAction("Index", "WishList");
        }
        public async Task<IActionResult> AddProductToWishList(string productNumber)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var isSuccess = await _favoriteService.AddProductToFavoritesAsync(productNumber);

                if (isSuccess)
                    TempData["success"] = "Product Added to the Wish List";

                return View();

            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = "Something went wrong";
            return View();
        }
    }
}
