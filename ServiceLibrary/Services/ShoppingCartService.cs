using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace ServiceLibrary.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        private readonly IJwtAuthenticationService _authenticationService;
        public ShoppingCartService(HttpClient httpClient, IJwtAuthenticationService authenticationService)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
        }
        public async Task<HttpResponseMessage> AddProductToShoppingCartAsync(int itemQuantity, string productNumber)
        {

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://localhost:7047/user/cart/add?quantity={itemQuantity}&productNumber={productNumber}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _authenticationService.RefreshTokenIfExpired());
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new HttpResponseMessage(statusCode: HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
        }
        public async Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync()
        {

            var shoppingCartProducts = new List<ShoppingCartProduct>();

            try
            {
                var apiUrl = $"https://localhost:7047/user/cart/products";
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiUrl),
                    Method = HttpMethod.Get,
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _authenticationService.RefreshTokenIfExpired());
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    shoppingCartProducts = JsonConvert.DeserializeObject<List<ShoppingCartProduct>>(responseBody);
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }

            return shoppingCartProducts;
        }
    }
}
