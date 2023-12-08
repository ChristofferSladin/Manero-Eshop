using ManeroWebApp.Controllers;
using ManeroWebApp.Models;
using ManeroWebApp.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Services;
using System.Net.Http.Headers;
using System.Text;
using UserAPI.Models;

namespace ManeroWebApp.Tests.ManeroApp.Controllers;

public class PromoCodesController : IClassFixture<CustomWebApplication>
{
    private readonly IPromoCodeService _promoCodeService;
    private readonly ManeroWebApp.Controllers.PromoCodesController _promoCodesController;
    private readonly HttpClient _httpClient;
    public PromoCodesController(CustomWebApplication customWeb)
    {
        _httpClient = customWeb.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7047/");
        _promoCodeService = new PromoCodeService(_httpClient);
        _promoCodesController = new ManeroWebApp.Controllers.PromoCodesController(_promoCodeService);
    }
    [Fact]
    public async Task Index_Should_ReturnToIndexView_When_PromoCodesListIsNotEmpty()
    {
        //Arrange
        var user = new { email = "customer1@customer.com", password = "Customer123#" };
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var loginResponse = await _httpClient.PostAsync("/login", content);
        var responseBody = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        //Act
        var result = await _promoCodesController.Index("current") as ViewResult;

        //Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);
    }
}
