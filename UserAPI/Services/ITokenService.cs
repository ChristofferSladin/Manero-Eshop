using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserAPI.Dtos;
using UserAPI.Models;

namespace UserAPI.Services
{
    public interface ITokenService
    {
        ClaimsPrincipal? GetClaimsPrincipal(string? token);
        Task<bool> RevokeRefreshTokenAsync(ClaimsPrincipal principal);
        Task<JwtSecurityToken> GenerateTokenAsync(string name);
        Task<bool> ValidateRefreshTokenAsync(RefreshModel model, ClaimsPrincipal principal);
        Task<LoginResponse> GenerateResponseAsync(ClaimsPrincipal principal);
        Task<LoginResponse> GenerateResponseAsync(IdentityUser user);
        Task<IdentityUser> ValidateUserAsync(UserDto userDto);
    }
}
