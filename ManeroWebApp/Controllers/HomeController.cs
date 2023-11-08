using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.Diagnostics;

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
            var featuredProducts = new List<Product>();
            var onSaleProducts = new List<Product>();

            var products = await _productService.GetProductsWithReviewsAsync();

            featuredProducts = products.Where(p => p.IsFeatured == true).DistinctBy(p => p.ProductName).ToList();
            onSaleProducts = products.Where(p => p.IsOnSale == true).DistinctBy(p => p.ProductName).ToList();

            var viewModel = new HomeIndexViewModel
            {
                FeaturedProducts = new CarouselViewModel
                {
                    IdSuffix = "1",
                    Title = "Featured Products",
                    Products = featuredProducts
                },
                OnSaleProducts = new CarouselViewModel
                {
                    IdSuffix = "2",
                    Title = "On Sale Products",
                    Products = onSaleProducts
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