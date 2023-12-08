using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using UserAPI.Models;
using UserAPI.Tests.Helpers;

namespace UserAPI.Tests.Controllers;

public class PromoCodeController : IClassFixture<TestWebApplication>
{
    private readonly HttpClient _httpClient;
    public PromoCodeController(TestWebApplication testWebApplication)
    {
        _httpClient = testWebApplication.CreateClient();
    }
    [Fact]
    public async Task GetPromoCodesByUserAsync_Returns_Ok_Response()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var promoCodeResponse = await _httpClient.GetAsync("/user/promo-code");

        //Assert
        Assert.Equal(HttpStatusCode.OK, promoCodeResponse.StatusCode);
    }
    [Fact]
    public async Task LinkPromoCodeToUserAsync_Returns_NotFound_If_WrongPromoCodeTextProvided()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
        var promoCodeText = "test";

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var promoCodeResponse = await _httpClient.PostAsync($"/user/link/promo-code?promoCodeText={promoCodeText}", new StringContent(""));

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, promoCodeResponse.StatusCode);
    }
    [Fact]
    public async Task LinkPromoCodeToUserAsync_Returns_Conflict_If_PromoCodeIsAlreadyLinked()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
        var promoCodeText = "FallIsHereSale15Percent";

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        await _httpClient.PostAsync($"/user/link/promo-code?promoCodeText={promoCodeText}", new StringContent(""));
        var promoCodeResponse_2 = await _httpClient.PostAsync($"/user/link/promo-code?promoCodeText={promoCodeText}", new StringContent(""));

        //Assert
        Assert.Equal(HttpStatusCode.Conflict, promoCodeResponse_2.StatusCode);
        
    }
}
