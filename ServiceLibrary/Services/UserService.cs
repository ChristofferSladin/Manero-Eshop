using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;
using System.Text;

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
    public async Task<ShoppingCart> GetShoppingCartByUser(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://localhost:7047/shoppingCart?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result.ToString();
                var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(content);

                if (shoppingCart is not null)
                    return shoppingCart;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<ShoppingCartProduct> CreateShoppingCartProductEntry(int productId, int shoppingCartId)
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
                    .DeserializeObject<ShoppingCartProduct>(content);

                if (shoppingCartProduct is not null)
                    return shoppingCartProduct;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<FavoriteProduct> AddProductToWishList(int productId, string userId)
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
                    .DeserializeObject<FavoriteProduct>(content);

                if (favoriteProduct is not null)
                    return favoriteProduct;
            }
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return null!;
    }
    public async Task<UserProfile> GetUserProfileAsync(string id)
    {
        var userProfile = new UserProfile();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7047/user/profile?id={id}"),
                Method = HttpMethod.Get,
            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                userProfile = JsonConvert.DeserializeObject<UserProfile>(responseString);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return userProfile;
    }
    public async Task<bool> RemoveProductFromWishListAsync(int productId, string userId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://localhost:7047/wishList/removeProduct?productId={productId}&userId={userId}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
        }
        catch (Exception e) { Debug.WriteLine(e.Message); }

        return false;
    }

    public async Task<string> GetUserToken(string email, string password)
    {
        var userLoginDto = new
        {
            Email = email,
            Password = password
        };
        try
        {
            var apiUrl = "https://localhost:7047/login";
            using var client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);

            var jsonContent = JsonConvert.SerializeObject(userLoginDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
        return null!;
    }
    public async Task<User> GetIdentityUser()
    {
        var user = new User();

        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://localhost:7047/user"),
                Method = HttpMethod.Get
            };

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responstring = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(responstring);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
        return user!;
    }

}
