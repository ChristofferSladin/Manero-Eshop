using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UserAPI.Dtos;
using UserAPI.Models;
using UserAPI.Tests.Helpers;

namespace UserAPI.Tests.Controllers;

public class FavoriteProductsController : IClassFixture<TestWebApplication>
{
    private readonly HttpClient _httpClient;
    public FavoriteProductsController(TestWebApplication testWebApplication)
    {
        _httpClient = testWebApplication.CreateClient();
    }
    [Fact]
    public async Task GetFavoriteProductsByUserAsync_Returns_Ok()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var favoriteResponse = await _httpClient.GetAsync("/user/favorite/products");

        //Assert
        Assert.Equal(HttpStatusCode.OK, favoriteResponse.StatusCode);
    }
    [Fact]
    public async Task GetFavoriteProductsByUserAsync_Returns_Unauthorized_If_NoAccessTokenProvided()
    {
        //Arrange


        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", null);
        var favoriteResponse = await _httpClient.GetAsync("/user/favorite/products");

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, favoriteResponse.StatusCode);
    }
    [Fact]
    public async Task AddProductToFavoritesAsync_Returns_Ok_Response()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
        var productNumber = "000000000001";

        //Act
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var favoriteResponse = await _httpClient.PostAsync($"/user/favorite/add?productNumber={productNumber}", new StringContent(""));

        //Assert
        Assert.Equal(HttpStatusCode.OK, favoriteResponse.StatusCode);
    }
}
