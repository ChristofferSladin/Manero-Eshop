using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> AddProductToShoppingCartAsync(string user, int itemQuantity, string productNumber)
        {

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://localhost:7047/user/cart/add?user={user}&quantity={itemQuantity}&productNumber={productNumber}");
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
        public async Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync(string user)
        {
            var shoppingCartProducts = new List<ShoppingCartProduct>();

            try
            {
                var apiUrl = $"https://localhost:7047/user/cart/products?user={user}";
                using var client = new HttpClient();

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(apiUrl);
                request.Method = HttpMethod.Get;
                var response = await client.GetAsync(apiUrl);

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
