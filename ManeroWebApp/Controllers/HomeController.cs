using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Net;

namespace ManeroWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public HomeController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {

            //Behöver läggas på en service!
            var cookie = Request.Cookies["UserHasSelectedLogin"];
            if (cookie == null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(69),
                };
                Response.Cookies.Append("UserHasSelectedLogin", "", cookieOptions);
                return RedirectToAction("Index", "WelcomePage");
            }

            var onSaleProducts = await _productService.GetOnSaleProductsWithReviewsAsync();
            var featuredProducts = await _productService.GetFeaturedProductsWithReviewsAsync();

            var viewModel = new HomeIndexViewModel
            {
                FeaturedProducts = new CarouselViewModel
                {
                    IdSuffix = "1",
                    Title = "Featured Products",
                    EndPoint = "FeaturedProducts",
                    Products = featuredProducts.Select(p => new ProductViewModel
                    {
                        ProductNumber = p.ProductNumber,
                        ProductName = p.ProductName,
                        Category = p.Category,
                        PriceExcTax = p.PriceExcTax,
                        PriceIncTax = p.PriceIncTax,
                        SalePricePercentage = p.SalePricePercentage,
                        IsOnSale = p.IsOnSale,
                        IsFeatured = p.IsFeatured,
                        Rating = p.Rating,
                        ImageUrl = p.ImageUrl,
                    }).DistinctBy(p => p.ProductName).ToList()
                },
                OnSaleProducts = new CarouselViewModel
                {
                    IdSuffix = "2",
                    Title = "On Sale Products",
                    EndPoint = "OnSaleProducts",
                    Products = onSaleProducts.Select(p => new ProductViewModel
                    {
                        ProductNumber = p.ProductNumber,
                        ProductName = p.ProductName,
                        Category = p.Category,
                        PriceExcTax = p.PriceExcTax,
                        PriceIncTax = p.PriceIncTax,
                        SalePricePercentage = p.SalePricePercentage,
                        IsOnSale = p.IsOnSale,
                        IsFeatured = p.IsFeatured,
                        Rating = p.Rating,
                        ImageUrl = p.ImageUrl,
                    }).DistinctBy(p => p.ProductName).ToList()
                }
            };
            return View(viewModel);
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}