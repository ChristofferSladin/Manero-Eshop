using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Contexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceLibrary.Models;

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
        public async Task<HttpResponseMessage> AddProductToShoppingCartAsync(string token, int itemQuantity, string productNumber)
        {
            await _authenticationService.TokenExpired(token);

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://localhost:7047/user/cart/add?quantity={itemQuantity}&productNumber={productNumber}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync(string token)
        {
            await _authenticationService.TokenExpired(token);
            var shoppingCartProducts = new List<ShoppingCartProduct>();

            try
            {
                var apiUrl = $"https://localhost:7047/user/cart/products";
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiUrl),
                    Method = HttpMethod.Get,
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
