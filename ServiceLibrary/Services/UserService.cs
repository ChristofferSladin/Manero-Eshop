﻿using DataAccessLibrary.Entities.ProductEntities;
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


    public async Task<bool> CheckApiStatusAsync()
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7047/health")
            };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return false;
        }
        return false;
    }
}
