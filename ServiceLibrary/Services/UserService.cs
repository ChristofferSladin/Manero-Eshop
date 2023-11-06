using Azure.Core;
using DataAccessLibrary.Repositories;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;

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
}
