using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Dtos;
using UserAPI.Models;
using UserAPI.Services;

namespace UserAPI.Controllers;

[Route("[controller]")]
[ApiController]
[EnableCors("AllowAll")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Authenticate user
    /// </summary>
    /// <returns>
    /// LoginResponse
    /// </returns>
    /// <remarks>
    /// Example end point: POST /login
    /// </remarks>
    /// <response code="200">
    /// Successfully authenticated user
    /// </response>
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

    /// <summary>
    /// Refresh user authentication
    /// </summary>
    /// <returns>
    /// LoginResponse
    /// </returns>
    /// <remarks>
    /// Example end point: POST /refresh
    /// </remarks>
    /// <response code="200">
    /// Successfully refreshed user authentication
    /// </response>
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

    /// <summary>
    /// Revoke user authentication
    /// </summary>
    /// <returns>
    /// Bool
    /// </returns>
    /// <remarks>
    /// Example end point: POST /revoke
    /// </remarks>
    /// <response code="200">
    /// Successfully revoked user authentication
    /// </response>
    [HttpPost]
    [Route("/revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshModel model)
    {
        if (!ModelState.IsValid) return Unauthorized();

        var principal = _tokenService.GetClaimsPrincipal(model.AccessToken);
        if (principal?.Identity?.Name == null!) return Unauthorized();

        var revoke = await _tokenService.RevokeRefreshTokenAsync(principal);
        if (revoke == false) return Unauthorized();

        return Ok(revoke);
    }
}
