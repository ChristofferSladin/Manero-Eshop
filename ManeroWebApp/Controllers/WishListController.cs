using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Principal;

namespace ManeroWebApp.Controllers
{
    public class WishListController : Controller
    {
        private readonly ManeroDbContext _context;
        public WishListController(ManeroDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string forUserId = "1e776705-a04e-4f48-9563-d548cf5db096") //user id hard coded for testing
        {
            var wishList = _context.Favorite.Include(fp => fp.FavoriteProducts)!
                .ThenInclude(p => p.Product)
                .FirstOrDefault(f => f.Id == forUserId);
            
            if (wishList is not null)
            {
                var favProductList = wishList.FavoriteProducts!
                    .Select(x => new FavoriteProductViewModel
                    {
                        UserId = forUserId,
                        ProductId = x.Product.ProductId,
                        ImgUrl = x.Product.ImageUrl,
                        Name = x.Product.ProductName,
                        SalePrice = x.Product.SalePrice,
                        PriceWithTax = x.Product.PriceIncTax,
                        PriceWithoutTax = x.Product.PriceExcTax,
                        Rating = x.Product.Rating,
                        IsOnSale = x.Product.IsOnSale,
                        ShoppingCartId = _context.ShoppingCarts
                        .FirstOrDefault(x => x.Id == forUserId)!.ShoppingCartId,
                    }).ToList();

                if (favProductList is not null)
                    return View(favProductList);
            }

            return View();
        }
        public async Task<IActionResult> AddProductToShoppingCart(int productId, int shoppingCartId)
        {
            try 
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(x => x.ProductId == productId);
                
                var shoppingCart = await _context.ShoppingCarts
                    .FirstOrDefaultAsync(x => x.ShoppingCartId == shoppingCartId);

                if (product != null && shoppingCart != null)
                {
                    var toAdd = new ShoppingCartProduct
                    {
                        Product = product,
                        ShoppingCart = shoppingCart,
                        ProductId = productId,
                        ShoppingCartId = shoppingCartId,
                        ItemQuantity = 1,
                    };

                    await _context.ShoppingCartProducts.AddAsync(toAdd);
                    await _context.SaveChangesAsync();

                    TempData["success"] = $"\"{product.ProductName}\" added to shopping cart.";
                }
            
            } catch(Exception e) { Debug.WriteLine(e.Message); }

            TempData["error"] = $"Something went wrong.";
            return RedirectToAction("Index", "WishList");
        }
    }
}
