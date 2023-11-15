using DataAccessLibrary.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserAPI.DTO;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var existingUser = await _userManager.FindByEmailAsync(userLoginDto.Email!);
            if (existingUser is not null)
            {
                var passwordVerification = await _userManager.CheckPasswordAsync(existingUser, userLoginDto.Password!);
                if (passwordVerification)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:LoginKey"]!));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var userRoles = await _userManager.GetRolesAsync(existingUser);

                    var claimsList = new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, existingUser.Id!),
                        new Claim(ClaimTypes.Name, existingUser.Email!),
                        new Claim(ClaimTypes.Email, existingUser.Email!),
                    };

                    foreach (var role in userRoles)
                    {
                        int newSize = claimsList.Length + 1;
                        Array.Resize(ref claimsList, newSize);
                        claimsList[newSize - 1] = new Claim(ClaimTypes.Role, role);
                    }

                    if (existingUser.UserName != null)
                    {
                        var token = await GenerateJwtToken(existingUser.UserName);

                        var refreshToken = GenerateRefreshToken();
                        existingUser.RefreshToken = refreshToken;
                        existingUser.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(2);
                        await _userManager.UpdateAsync(existingUser);

                        var loginResponse = new LoginResponse
                        {
                            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo,
                            RefreshToken = refreshToken
                        };

                        return Ok(loginResponse);
                    }
                }
            }
            return Problem();
        }

        [HttpPost]
        [Route("/refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal?.Identity?.Name == null!)
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            //if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            //{
            //    return Unauthorized();
            //}
            var token = await GenerateJwtToken(principal.Identity.Name);

            var loginResponse = new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = model.RefreshToken
            };

            return Ok(loginResponse);
        }
        [Authorize]
        [HttpDelete]
        [Route("/revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = HttpContext.User.Identity?.Name;

            if (username is null)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return Unauthorized();

            //user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return Ok();
        }
        private async Task<JwtSecurityToken> GenerateJwtToken(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:LoginKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsList = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                int newSize = claimsList.Length + 1;
                Array.Resize(ref claimsList, newSize);
                claimsList[newSize - 1] = new Claim(ClaimTypes.Role, role);
            }

            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claimsList,
                    expires: DateTime.UtcNow.AddMinutes(1),
                    signingCredentials: credentials);

            return token;
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var vailidation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:LoginKey"]!)),
                ValidateLifetime = false
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, vailidation, out _);
        }
        private static string GenerateRefreshToken()
        {
            var randNum = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randNum);
            return Convert.ToBase64String(randNum);
        }
    }
}
