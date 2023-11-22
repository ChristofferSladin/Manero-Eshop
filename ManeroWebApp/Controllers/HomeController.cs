using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await _shoppingCartService.IncrementProductInShoppingCartAsync(increment, productNumber);
            }
            else
            {
                var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
                var shoppingCartCookie = Request.Cookies["ShoppingCart"];
                if (shoppingCartCookie != null)
                {
                    var shoppingCartItems = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(shoppingCartCookie);
                    var product = shoppingCartItems?.FirstOrDefault(s => s.ProductNumber == productNumber);
                    if (product != null)
                    {
                        switch (increment)
                        {
                            case Increment.Add:
                                product.ItemQuantity += 1;
                                break;
                            case Increment.Remove:
                                if (product.ItemQuantity > 1)
                                {
                                    product.ItemQuantity -= 1;
                                }
                                break;
                        }
                    }
                    Response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(shoppingCartItems), cookieOptions);
                }
            }
            return RedirectToAction("HeaderPartial", "Home");
        }
        public async Task<IActionResult> RemoveProductFromShoppingCartAsync(string productNumber)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await _shoppingCartService.RemoveProductFromShoppingCartAsync(productNumber);
            }
            else
            {
                var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
                var shoppingCartCookie = Request.Cookies["ShoppingCart"];
                if (shoppingCartCookie != null)
                {
                    var shoppingCartItems = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(shoppingCartCookie);
                    shoppingCartItems?.RemoveAll(s => s.ProductNumber == productNumber);
                    Response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(shoppingCartItems), cookieOptions);
                }
            }
            return RedirectToAction("HeaderPartial", "Home");
        }

        public IActionResult HeaderPartial()
        {
            return PartialView("/Views/Shared/Header/_Header.cshtml");
        }

        public async Task<IActionResult> ShoppingCartPartial()
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
                    ShoppingCartViewModel shoppingCartViewModel = product;
                    homeIndexViewModel.TestModel.ShoppingCartProducts.Add(shoppingCartViewModel);
                    shoppingCartViewModel.ItemQuantity = cartProduct.ItemQuantity;
                }
            }
            else
            {
                var shoppingCartCookie = Request.Cookies["ShoppingCart"];
                if (shoppingCartCookie != null)
                {
                    var shoppingCartItems = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(shoppingCartCookie);
                    if (shoppingCartItems != null)
                    {
                        foreach (var cartProduct in shoppingCartItems)
                        {
                            var product = await _productService.GetProductAsync(cartProduct.ProductNumber);
                            ShoppingCartViewModel shoppingCartViewModel = product;
                            homeIndexViewModel.TestModel.ShoppingCartProducts.Add(shoppingCartViewModel);
                            shoppingCartViewModel.ItemQuantity = cartProduct.ItemQuantity;
                        }
                    }
                }
            }

            return PartialView("/Views/Shared/Header/_ShoppingCart.cshtml", homeIndexViewModel);
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