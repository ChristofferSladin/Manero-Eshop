using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI.DTO;

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
                        new Claim(ClaimTypes.NameIdentifier, existingUser.Email!),
                        new Claim(ClaimTypes.Email, existingUser.Email!),
                    };

                    foreach (var role in userRoles)
                    {
                        int newSize = claimsList.Length + 1;
                        Array.Resize(ref claimsList, newSize);
                        claimsList[newSize - 1] = new Claim(ClaimTypes.Role, role);
                    }

                    var securityToken = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claimsList,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                    return Ok(token);
                }
            }
            return Problem();
        }
    }
}
