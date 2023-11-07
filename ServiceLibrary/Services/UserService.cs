using Azure.Core;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using DataAccessLibrary.Entities.UserEntities;
using FavoriteProduct = ServiceLibrary.Models.FavoriteProduct;

namespace ServiceLibrary.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<FavoriteProduct>> GetAllFavoriteProductsForUserAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://localhost:7047/favoriteProducts?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var favProductsList = JsonConvert.DeserializeObject<IEnumerable<FavoriteProduct>>(content);

                if (favProductsList is not null)
                    return favProductsList;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<DataAccessLibrary.Entities.UserEntities.ShoppingCart> GetShoppingCartByUser(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://localhost:7047/shoppingCart?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var shoppingCart = JsonConvert.DeserializeObject<DataAccessLibrary.Entities.UserEntities.ShoppingCart>(content);

                if (shoppingCart is not null)
                    return shoppingCart;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<DataAccessLibrary.Entities.UserEntities.ShoppingCartProduct> CreateShoppingCartProductEntry(int productId, int shoppingCartId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://localhost:7047/createShoppingCartProduct?productId={productId}&shoppingCartId={shoppingCartId}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var shoppingCartProduct = JsonConvert
                    .DeserializeObject<DataAccessLibrary.Entities.UserEntities.ShoppingCartProduct>(content);

                if (shoppingCartProduct is not null)
                    return shoppingCartProduct;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<DataAccessLibrary.Entities.UserEntities.FavoriteProduct> AddProductToWishList(int productId, string userId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://localhost:7047/createFavoriteProduct?productId={productId}&userId={userId}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var favoriteProduct = JsonConvert
                    .DeserializeObject<DataAccessLibrary.Entities.UserEntities.FavoriteProduct>(content);

                if (favoriteProduct is not null)
                    return favoriteProduct;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }

    public async Task<Models.UserProfile> GetUserProfileAsync(string id)
    {
        var userProfile = new Models.UserProfile();
        var uId = $"?id={id}";
        try
        {
            var baseUrl = $"https://localhost:7047/user/profile{uId}";
            using var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(baseUrl);
            request.Method = HttpMethod.Get;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                userProfile = JsonConvert.DeserializeObject<Models.UserProfile>(responseString);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return userProfile;
    }
}
