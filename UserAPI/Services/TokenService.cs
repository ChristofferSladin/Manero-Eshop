using DataAccessLibrary.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Dtos;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ManeroDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TokenService(IConfiguration configuration, ManeroDbContext context, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        public ClaimsPrincipal? GetClaimsPrincipal(string? token)
        {
            var validation = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:LoginKey"]!)),
                ValidateLifetime = false
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
        public async Task<string> GenerateRefreshToken(IdentityUser user)
        {
            var num = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(num);
            var refreshToken = Convert.ToBase64String(num);

            var userRefreshToken = await _context.UserRefreshToken.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userRefreshToken != null)
            {
                userRefreshToken.RefreshToken = refreshToken;
                userRefreshToken.RefreshTokenExpiry = DateTime.UtcNow.AddMonths(12);
                await _context.SaveChangesAsync();
            }
            return refreshToken;
        }

        public async Task<bool> RevokeRefreshTokenAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.Name == null) return false;
            var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
            var refreshToken = await _context.UserRefreshToken.FirstOrDefaultAsync(u => u.Id == user!.Id);
            if (refreshToken == null) return false;
            refreshToken.RefreshToken = null;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<JwtSecurityToken> GenerateTokenAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (user == null) return null!;
            var claimsList = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Email, user.Email!),
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                var newSize = claimsList.Length + 1;
                Array.Resize(ref claimsList, newSize);
                claimsList[newSize - 1] = new Claim(ClaimTypes.Role, role);
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claimsList,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            return token;
        }

        public async Task<bool> ValidateRefreshTokenAsync(RefreshModel model, ClaimsPrincipal principal)
        {

            if (principal.Identity?.Name == null) return false;
            var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
            if (user == null) return false;
            var refreshToken = await _context.UserRefreshToken.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (refreshToken == null) return false;
            return refreshToken.RefreshToken == model.RefreshToken && refreshToken.RefreshTokenExpiry >= DateTime.UtcNow;
        }

        public async Task<LoginResponse> GenerateResponseAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.Name == null) return null!;
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null) return null!;
            var refreshToken = await GenerateRefreshToken(user);
            var token = await GenerateTokenAsync(principal.Identity.Name);
            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            };
        }
        public async Task<LoginResponse> GenerateResponseAsync(IdentityUser user)
        {
            if (user.UserName == null) return null!;
            var token = await GenerateTokenAsync(user.UserName);
            var refreshToken = await GenerateRefreshToken(user);

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            };
        }

        public async Task<IdentityUser> ValidateUserAsync(UserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email!);
            if (user == null) return null!;
            var password = await _userManager.CheckPasswordAsync(user, userDto.Password!);
            if (password == false) return null!;
            return user;
        }
    }
}
