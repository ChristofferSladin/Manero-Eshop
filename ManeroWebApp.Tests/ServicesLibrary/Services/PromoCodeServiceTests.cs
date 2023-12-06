using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Dtos;
using static System.Net.WebRequestMethods;

namespace ManeroWebApp.Tests.ServicesLibrary.Services
{
    public class PromoCodeServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly PromoCodeService _promoCodeService;
        private readonly List<PromoCodeDto> _promoCodeDtoList;
        public PromoCodeServiceTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandler.Object);
            _promoCodeService = new PromoCodeService(_httpClient);
            _promoCodeDtoList = new List<PromoCodeDto>
                {
                     new PromoCodeDto
                     {
                        PromoCodeName = "Sigma Apparel",
                        PromoCodePercentage = .30M,
                        PromoCodeAmount = null,
                        PromoCodeIsUsed = false,
                        PromoCodeValidity = DateTime.Now.AddDays(30),
                        PromoCodeText = "HurryUpSale23",
                        PromoCodeImgUrl = "https://images.unsplash.com/photo-1607082350899-7e105aa886ae?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    },
                    new PromoCodeDto
                    {
                        PromoCodeName = "Shoes4U",
                        PromoCodePercentage = .50M,
                        PromoCodeAmount = null,
                        PromoCodeIsUsed = false,
                        PromoCodeValidity = DateTime.Now.AddDays(15),
                        PromoCodeText = "BlackWeek2023",
                        PromoCodeImgUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?w=500&auto=format&fit=crop&q=60&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8YXBwYXJlbHxlbnwwfHwwfHx8MA%3D%3D",
                    },
                    new PromoCodeDto
                    {
                        PromoCodeName = "Union Pants co.",
                        PromoCodePercentage = .50M,
                        PromoCodeAmount = null,
                        PromoCodeIsUsed = true,
                        PromoCodeValidity = DateTime.Now.AddDays(-10),
                        PromoCodeText = "JustForYou12",
                        PromoCodeImgUrl = "https://images.unsplash.com/photo-1607082349566-187342175e2f?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    }
            };
        }
        
        [Fact]
        public async Task GetPromoCodesByUserAsync_Returns_ListOfAllPromoCodes()
        {
            // Arrange
            var successfulResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_promoCodeDtoList))
            };

            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x =>
                        x.RequestUri == new Uri("https://localhost:7047/user/promo-code")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(successfulResponse);


            // Act
            var promoCodes = await _promoCodeService.GetPromoCodesByUserAsync();

            // Assert
            Assert.NotNull(promoCodes);
            Assert.NotEmpty(promoCodes);
            Assert.IsType<List<PromoCode>>(promoCodes);
        }
        [Fact]
        public async Task GetPromoCodesByUserAsync_Returns_ListOfUnusedPromoCodes_When_StatusIsCurrent()
        {
            //Arrange
            var status = "current";
            var successfulResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_promoCodeDtoList))
            };

            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x =>
                        x.RequestUri == new Uri("https://localhost:7047/user/promo-code")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(successfulResponse);
            
            //Act
            var promoCodes = await _promoCodeService.GetPromoCodesByUserAsync(status);

            //Assert
            Assert.NotNull(promoCodes);
            Assert.NotEmpty(promoCodes);
            Assert.IsType<List<PromoCode>>(promoCodes);
            foreach(var promoCode in promoCodes)
            {
                Assert.False(promoCode.PromoCodeIsUsed);
            }
        }
        [Fact]
        public async Task GetPromoCodesByUserAsync_Returns_ListOfUnusedPromoCodes_When_StatusIsUsed()
        {
            //Arrange
            var status = "used";
            var successfulResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_promoCodeDtoList))
            };

            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x =>
                        x.RequestUri == new Uri("https://localhost:7047/user/promo-code")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(successfulResponse);

            //Act
            var promoCodes = await _promoCodeService.GetPromoCodesByUserAsync(status);

            //Assert
            Assert.NotNull(promoCodes);
            Assert.NotEmpty(promoCodes);
            Assert.IsType<List<PromoCode>>(promoCodes);
            foreach (var promoCode in promoCodes)
            {
                Assert.True(promoCode.PromoCodeIsUsed);
            }
        }
    }
}
