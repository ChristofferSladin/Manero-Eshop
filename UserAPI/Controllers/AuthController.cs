using DataAccessLibrary.Contexts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserAPI.Dtos;
using UserAPI.Models;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) return Unauthorized();

            var user = await _tokenService.ValidateUserAsync(userDto);
            if (user == null!) return Unauthorized();

            var response = await _tokenService.GenerateResponseAsync(user);
            return Ok(response);
        }

        [HttpPost]
        [Route("/refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
        {
            if (!ModelState.IsValid) return Unauthorized();

            var principal = _tokenService.GetClaimsPrincipal(model.AccessToken);
            if (principal?.Identity?.Name == null!) return Unauthorized();

            var validateRefreshToken = await _tokenService.ValidateRefreshTokenAsync(model, principal);
            if (validateRefreshToken == false) return Unauthorized();

            var response = await _tokenService.GenerateResponseAsync(principal);
            if (response == null!) return Unauthorized();

            return Ok(response);
        }

        [HttpPost]
        [Route("/revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshModel model)
        {
            if (!ModelState.IsValid) return Unauthorized();

            var principal = _tokenService.GetClaimsPrincipal(model.AccessToken);
            if (principal?.Identity?.Name == null!) return Unauthorized();

            var revoke = await _tokenService.RevokeRefreshTokenAsync(principal);
            if (revoke == false) return Unauthorized();

            return Ok();
        }
    }
}
