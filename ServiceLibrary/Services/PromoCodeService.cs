using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly HttpClient _httpClient;

        public PromoCodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<PromoCode>> GetPromoCodesByUserAsync()
        {
            List<PromoCode> promoCodes = new();
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:7047/user/promo-code"),
                    Method = HttpMethod.Get
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    promoCodes = JsonConvert.DeserializeObject<List<PromoCode>>(responseBody)!;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return promoCodes;
        }
        public async Task<IEnumerable<PromoCode>> GetPromoCodesByUserAsync(string status)
        {
            List<PromoCode> promoCodes = new();

            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:7047/user/promo-code"),
                    Method = HttpMethod.Get
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    promoCodes = JsonConvert.DeserializeObject<List<PromoCode>>(responseBody)!;
                    switch (status.ToLower())
                    {
                        case "current":
                            promoCodes = promoCodes.Where(x => x.PromoCodeIsUsed == false).ToList();
                            break;

                        case "used":
                            promoCodes = promoCodes.Where(x => x.PromoCodeIsUsed == true).ToList();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return promoCodes;
        }
        public async Task<string> LinkPromoCodeToUserAsync(string promoCodeText)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://localhost:7047/user/link/promo-code?promoCodeText={promoCodeText}"),
                    Method = HttpMethod.Post
                };
                var response = await _httpClient.SendAsync(request);
                var reponseMessage = await response.Content.ReadAsStringAsync();
                return reponseMessage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
            return null!;
        }
    }
}
