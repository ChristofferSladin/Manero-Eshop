using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ServiceLibrary.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ServiceLibrary.Services
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;
        public JwtAuthenticationService(IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        private async Task RevokeCookieTokensAndSignOut()
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
        public async Task<string> RefreshTokenIfExpired()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                await RevokeCookieTokensAndSignOut();
                return null!;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
            if (token != null && token.ValidTo <= DateTime.UtcNow.AddMinutes(1))
                await RefreshTokenAsync();
            return _httpContextAccessor.HttpContext.Request.Cookies["Token"];
        }

        public async Task<bool> GetTokenAsync(string email, string password)
        {
            var userLogin = new
            {
                Email = email,
                Password = password
            };
            try
            {
                var apiUrl = "https://localhost:7047/login";
                using var client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);

                var jsonContent = JsonConvert.SerializeObject(userLogin);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<RefreshModel>(responseString);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("Token", token!.AccessToken);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", token.RefreshToken);
                    return true;
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return false;
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var model = new RefreshModel
                {
                    AccessToken = _httpContextAccessor.HttpContext.Request.Cookies["Token"],
                    RefreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"]
                };

                var apiUrl = "https://localhost:7047/refresh";
                using var client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);

                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/refresh", content);
                switch (response.IsSuccessStatusCode)
                {
                    case false:
                        await RevokeTokenAsync();
                        await RevokeCookieTokensAndSignOut();
                        return false;
                    case true:
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var token = JsonConvert.DeserializeObject<RefreshModel>(responseString);
                            _httpContextAccessor.HttpContext.Response.Cookies.Append("Token", token!.AccessToken);
                            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", token!.RefreshToken);
                            return true;
                        }
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return false;
        }
        public async Task<bool> RevokeTokenAsync()
        {
            try
            {
                var model = new RefreshModel
                {
                    AccessToken = _httpContextAccessor.HttpContext.Request.Cookies["Token"],
                    RefreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"]
                };
                var apiUrl = "https://localhost:7047/revoke";
                using var client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/revoke", content);

                if (response.IsSuccessStatusCode)
                {
                    await RevokeCookieTokensAndSignOut();
                    return true;
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return false;
        }
    }
}
