using ManeroWebApp.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.Net.Http.Headers;
using System.Text;
using UserAPI.Models;

namespace ManeroWebApp.Tests;

public class Test : IClassFixture<CustomWebApplication>
{
    private readonly IPromoCodeService _promoCodeService;
    private readonly HttpClient _httpClient;
    public Test(CustomWebApplication customWeb)
    {
        _httpClient = customWeb.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7047/");
        _promoCodeService = new PromoCodeService(_httpClient);
    }
    [Fact]
    public async Task Testing()
    {
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        var result = await _promoCodeService.GetPromoCodesByUserAsync();

        Assert.NotNull(result);
        Assert.IsType<List<PromoCode>>(result);

    }
}
