using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;

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

        public async Task<IActionResult> IncrementShoppingCartProductAsync(Increment increment, string productNumber)
        {
            await _shoppingCartService.IncrementProductInShoppingCartAsync(increment, productNumber);
            return RedirectToAction("HeaderPartial", "Home");
        }
        public async Task<IActionResult> RemoveProductFromShoppingCartAsync(string productNumber)
        {
            await _shoppingCartService.RemoveProductFromShoppingCartAsync(productNumber);
            return RedirectToAction("HeaderPartial", "Home");
        }
        public async Task<IActionResult> HeaderPartial()
        {
            var cartProducts = await _shoppingCartService.GetUserShoppingCartProductsAsync();

            var homeIndexViewModel = new HomeIndexViewModel
            {
                TestModel = new TestingShoppingCartViewModel
                {
                    ShoppingCartProducts = new List<ShoppingCartViewModel>(),
                }
            };
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {

                foreach (var cartProduct in cartProducts)
                {
                    var product = await _productService.GetProductByIdAsync(cartProduct.ProductId);
                    var productViewModel = new ShoppingCartViewModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductNumber = product.ProductNumber,
                        Category = product.Category,
                        SalePricePercentage = product.SalePricePercentage,
                        ImageUrl = product.ImageUrl,
                        PriceExcTax = product.PriceExcTax,
                        PriceIncTax = product.PriceIncTax,
                        IsOnSale = product.IsOnSale,
                        ItemQuantity = cartProduct.ItemQuantity
                    };
                    homeIndexViewModel.TestModel.ShoppingCartProducts.Add(productViewModel);
                }
            }

            return PartialView("/Views/Shared/Header/_HeaderShoppingCart.cshtml", homeIndexViewModel);
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