using Microsoft.IdentityModel.Tokens;
using ServiceLibrary.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ManeroWebApp.Tests.ServicesLibrary.Services
{
    public class ShoppingCartServiceTests
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly HttpClient _httpClient = new();

        public ShoppingCartServiceTests()
        {
            _shoppingCartService = new(_httpClient);
        }

        [Fact]
        public async Task GetUserShoppingCartProducts_WithoutToken_Returns_UserShoppingCartProduct_With_No_Products()
        {
            var items = await _shoppingCartService.GetUserShoppingCartProductsAsync();

            Assert.NotNull(items);
            Assert.True(items.Count == 0);
        }

        [Fact]
        public async Task AddingItemToCart_WithoutToken_Returns_False()
        {
            var itemToAdd = new
            {
                ItemQuantity = 1,
                ProductNumber = 1.ToString("D12")
            };
       
            var response = await _shoppingCartService.AddProductToShoppingCartAsync(itemToAdd.ItemQuantity, itemToAdd.ProductNumber);
           
            Assert.True(response == false);
        }

        [Fact]
        public async Task GetUserShoppingCartProducts_WithInvalidToken_Returns_UserShoppingCartProduct_With_No_Products()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalidToken");
            var items = await _shoppingCartService.GetUserShoppingCartProductsAsync();

            Assert.NotNull(items);
            Assert.True(items.Count == 0);
        }
    }
}
