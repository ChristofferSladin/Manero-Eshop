using DataAccessLibrary.Contexts;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Entities.UserEntities;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ManeroWebApp.Controllers
{
    public class WishListController : Controller
    {
        private readonly ManeroDbContext _context;
        private readonly HttpClient _httpClient;
        public WishListController(ManeroDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }
        public IActionResult WishList(string forUserId = "6c0d8713-a32a-4209-a79b-6744ca401197") //user id hard coded for testing
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
                        Price = x.Product.SalePrice,
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
        public async Task<IActionResult> AddProductToShoppingCart(int productId, int shoppingCartId, decimal priceWithTax, decimal priceWithoutTax)
        {
            var shoppingCardProductEntry = new ShoppingCartProduct
            {
                TotalPriceExcTax = priceWithoutTax,
                TotalPriceIncTax = priceWithTax,
                Product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId)!,
                ShoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.ShoppingCartId == shoppingCartId)!,
                ItemQuantity = 1,
            };

            await _context.ShoppingCartProducts
                .AddAsync(shoppingCardProductEntry);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Home");
        }
    }
}
