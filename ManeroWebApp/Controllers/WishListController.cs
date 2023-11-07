using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Security.Principal;

namespace ManeroWebApp.Controllers
{
    public class WishListController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public WishListController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }
        public async Task<IActionResult> Index(string forUserId = "1e776705-a04e-4f48-9563-d548cf5db096") //user id hard coded for testing
        {
            try
            {
                var favProductsByUser = await _userService.GetAllFavoriteProductsForUserAsync(forUserId);

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
    }
}
