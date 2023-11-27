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
}
