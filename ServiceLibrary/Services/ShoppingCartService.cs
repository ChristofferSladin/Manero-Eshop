using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;

namespace ServiceLibrary.Services;
public enum Increment
{
    Default,
    Add,
    Remove
}
public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient _httpClient;
    public ShoppingCartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<bool> IncrementProductInShoppingCartAsync(Increment increment, string productNumber)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7047/user/cart/increment?increment={increment}&productNumber={productNumber}"),
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
    public async Task<bool> RemoveProductFromShoppingCartAsync(string productNumber)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7047/user/cart/remove?productNumber={productNumber}"),
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
    public async Task<bool> AddProductToShoppingCartAsync(int itemQuantity, string productNumber)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7047/user/cart/add?quantity={itemQuantity}&productNumber={productNumber}"),
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
    public async Task<List<ShoppingCartProduct>> GetUserShoppingCartProductsAsync()
    {
        var shoppingCartProducts = new List<ShoppingCartProduct>();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://localhost:7047/user/cart/products"),
                Method = HttpMethod.Get,
            };
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
