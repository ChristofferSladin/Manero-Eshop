using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;

namespace ServiceLibrary.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly HttpClient _httpClient;
        public FavoriteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> RemoveProductToFavoritesAsync(string productNumber)
        {

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:7047/user/favorite/remove?productNumber={productNumber}"),
                    Method = HttpMethod.Post
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return false;
        }
        public async Task<bool> AddProductToFavoritesAsync(string productNumber)
        {

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:7047/user/favorite/add?productNumber={productNumber}"),
                    Method = HttpMethod.Post
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return false;
        }
        public async Task<List<FavoriteProduct>> GetUserFavoriteProductsAsync()
        {
            var favoriteProducts = new List<FavoriteProduct>();

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://localhost:7047/user/favorite/products"),
                    Method = HttpMethod.Get,
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    favoriteProducts = JsonConvert.DeserializeObject<List<FavoriteProduct>>(responseBody);
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

            return favoriteProducts;
        }
    }
}
