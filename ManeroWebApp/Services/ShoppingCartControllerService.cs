using System.Diagnostics;
using System.Security.Claims;
using Azure;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Services;

namespace ManeroWebApp.Services
{
    public class ShoppingCartControllerService : IShoppingCartControllerService
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartControllerService(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;

            _shoppingCartService = shoppingCartService;
        }
        public async Task<List<ShoppingCartViewModel>> GetShoppingForUserCart(string user)
        {
            var shoppingCart = new List<ShoppingCartViewModel>();
            try
            {

                var shoppingCartItems = await _shoppingCartService.GetUserShoppingCartProductsAsync(user);
                foreach (var cartItem in shoppingCartItems)
                {
                    var item = await _productService.GetProductByIdAsync(cartItem.ProductId);
                    ShoppingCartViewModel cartViewModel = item;
                    cartViewModel.ItemQuantity = cartItem.ItemQuantity;
                    shoppingCart.Add(cartViewModel);
                }

                return shoppingCart;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return new List<ShoppingCartViewModel>();
        }
        public async Task<List<ShoppingCartViewModel>> GetShoppingForGuestCart(string shoppingCartCookie)
        {
            var shoppingCart = new List<ShoppingCartViewModel>();
            try
            {

                var shoppingCartItems = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(shoppingCartCookie);
                foreach (var cartItem in shoppingCartItems)
                {
                    var item = await _productService.GetProductAsync(cartItem.ProductNumber);
                    ShoppingCartViewModel cartViewModel = item;
                    cartViewModel.ItemQuantity = cartItem.ItemQuantity;
                    shoppingCart.Add(cartViewModel);
                }

                return shoppingCart;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return new List<ShoppingCartViewModel>();
        }
        public async Task AddProductToShoppingCartForUser(string user, int itemQuantity, string productNumber)
        {
            await _shoppingCartService.AddProductToShoppingCartAsync(user, itemQuantity, productNumber);
        }
        public void AddProductToShoppingCartForGuest(HttpResponse response, string? existingShoppingCartCookie, int itemQuantity, string productNumber)
        {
            var shoppingCart = new ShoppingCartItems
            {
                ProductNumber = productNumber,
                ItemQuantity = itemQuantity
            };
            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };

            if (!string.IsNullOrEmpty(existingShoppingCartCookie))
            {
                var existingShoppingCart = JsonConvert.DeserializeObject<List<ShoppingCartItems>>(existingShoppingCartCookie);
                if (existingShoppingCart != null)
                {
                    var existingItem = existingShoppingCart.FirstOrDefault(item => item.ProductNumber == productNumber);

                    if (existingItem != null)
                    {
                        existingItem.ItemQuantity += itemQuantity;
                    }
                    else
                    {
                        existingShoppingCart.Add(shoppingCart);
                    }
                }

                response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(existingShoppingCart), cookieOptions);
            }
            else
            {
                var newShoppingCart = new List<ShoppingCartItems> { shoppingCart };
                response.Cookies.Append("ShoppingCart", JsonConvert.SerializeObject(newShoppingCart), cookieOptions);
            }
        }

    }
}
