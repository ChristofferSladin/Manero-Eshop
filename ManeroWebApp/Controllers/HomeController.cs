using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Net;

namespace ManeroWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
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

        public async Task<IActionResult> HeaderPartial()
        {
            var listOfProd = await _productService.GetProductsWithReviewsAsync();

            var list = new HomeIndexViewModel
            {
                TestModel = new TestingShoppingCartViewModel
                {
                    products = new List<ProductViewModel>()
                }
            };

            foreach (var prod in listOfProd)
            {
                var productViewModel = new ProductViewModel
                {
                    ProductId = prod.ProductId,
                    ProductName = prod.ProductName,
                    ProductNumber = prod.ProductNumber,
                    Category = prod.Category,
                    SalePricePercentage = prod.SalePricePercentage,
                    ImageUrl = prod.ImageUrl,
                    PriceExcTax = prod.PriceExcTax,
                    PriceIncTax = prod.PriceIncTax,
                    Description = prod.Description,
                    IsOnSale = prod.IsOnSale,
                };
                list.TestModel.products.Add(productViewModel);
            }

            return PartialView("/Views/Shared/Header/_HeaderShoppingCart.cshtml", list);
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