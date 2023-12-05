using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ServiceLibrary.Services
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly HttpClient _httpClient;

        public JwtAuthService(IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _httpClient = httpClient;
        }


        public async Task<bool> RenewTokenIfExpiredAsync()
        {
            try
            {
                var accessToken = RetrieveCookieTokens().AccessToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                if (_httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
                {
                    if (tokenHandler.ReadToken(accessToken) is JwtSecurityToken token)
                    {
                        if (token.ValidTo > DateTime.UtcNow) return true;
                        if (await RefreshTokenAsync()) return true;
                    }
                }
                await RevokeTokenAsync();
                await RevokeCookieTokensAndSignOutAsync();
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await RevokeCookieTokensAndSignOutAsync();
                return false;
            }
        }

        public async Task<bool> GetTokenAsync(string email, string password)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(new { Email = email, Password = password });
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7047/login"),
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return false;
                var responseString = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<RefreshModel>(responseString);
                UpdateCookieTokens(token);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(RetrieveCookieTokens());
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7047/refresh"),
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode) return false;
                var responseString = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<RefreshModel>(responseString);
                UpdateCookieTokens(token);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                return false;
            }
        }
        public async Task<bool> RevokeTokenAsync()
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(RetrieveCookieTokens());
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7047/revoke"),
                    Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                };
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode) return true;
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return false;
        }


        public void UpdateCookieTokens(RefreshModel? token)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Token", token!.AccessToken);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", token!.RefreshToken);
        }

        public RefreshModel RetrieveCookieTokens()
        {
            return new RefreshModel
            {
                AccessToken = _httpContextAccessor.HttpContext.Request.Cookies["Token"],
                RefreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"]
            };
        }
        private async Task RevokeCookieTokensAndSignOutAsync()
        {
            try
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("Token");
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");
                await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
