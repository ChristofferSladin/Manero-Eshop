using ManeroWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace ManeroWebApp.Controllers
{
    public class WishListController : Controller
    {
        private readonly IUserService _userService;
        public WishListController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var favProductsByUser = await _userService.GetAllFavoriteProductsForUserAsync(userId);
                
                if (favProductsByUser is not null)
                    return View(favProductsByUser.Select(favProduct => (FavoriteProductViewModel)favProduct));
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return View();
        }
        public async Task<IActionResult> AddProductToShoppingCart(int productId, int shoppingCartId)
        {
            try
            {
                var result = await _userService.CreateShoppingCartProductEntry(productId, shoppingCartId);
                if (result is not null)
                {
                    TempData["success"] = $"Product added to the shopping cart.";
                    return RedirectToAction("Index", "WishList");
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = $"Something went wrong.";
            return RedirectToAction("Index", "WishList");
        }
        public async Task<IActionResult> RemoveProductFromWishList(int productId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var isSuccess = await _userService.RemoveProductFromWishListAsync(productId, userId);
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
        public async Task<IActionResult> AddProductToWishList(int productId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var result = await _userService.AddProductToWishList(productId, userId);

                if (result is not null)
                    TempData["success"] = "Product Added to the Wish List";

                return View();

            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = "Something went wrong";
            return View();
        }
    }
}
