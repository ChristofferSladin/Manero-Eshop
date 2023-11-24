using Azure.Core;
using Azure;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Services;

namespace ManeroWebApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IProductService _productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
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
            return RedirectToAction("HeaderPartial", "Header");
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
            return RedirectToAction("HeaderPartial", "Header");
        }

        public async Task<IActionResult> ShoppingCartPartial()
        {
            var homeIndexViewModel = new HomeIndexViewModel
            {
                TestModel = new TestingShoppingCartViewModel
                {
                    ShoppingCartProducts = new List<ShoppingCartViewModel>(),
                }
            };
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var cartProducts = await _shoppingCartService.GetUserShoppingCartProductsAsync();

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
    }
}
