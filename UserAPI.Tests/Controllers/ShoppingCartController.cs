using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Models;
using UserAPI.Tests.Helpers;

namespace UserAPI.Tests.Controllers
{
    public class ShoppingCartController : IClassFixture<TestWebApplication>
    {
        private readonly HttpClient _httpClient;
        public ShoppingCartController(TestWebApplication testWebApplication)
        {
            _httpClient = testWebApplication.CreateClient();
        }
        [Fact]
        public async Task RemoveProductFromShoppingCartAsync_Returns_FalseText_If_ProductNumberIsWrong()
        {
            //Arrange
            var user = new { email = "customer1@customer.com", password = "Customer123#" };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync("/login", content);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
            var productNumber = "test";

            //Act
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsync($"/user/cart/remove?productNumber={productNumber}", new StringContent(""));
            var responseMsg = response.Content.ReadAsStringAsync().Result;

            //Assert
            Assert.Equal("false", responseMsg);
        }
        [Fact]
        public async Task RemoveProductFromShoppingCartAsync_Returns_BadRequest_If_ProductNumberIsNotProvided()
        {
            //Arrange
            var user = new { email = "customer1@customer.com", password = "Customer123#" };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync("/login", content);
            var responseBody = await loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<LoginResponse>(responseBody)!.AccessToken;
            var productNumber = "";

            //Act
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await _httpClient.PostAsync($"/user/cart/remove?productNumber={productNumber}", new StringContent(""));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
