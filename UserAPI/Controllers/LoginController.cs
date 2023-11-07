using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DTO;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LoginController : ControllerBase
    {
        //private readonly IConfiguration _configuration;
        //private readonly UserRepository _userRepository;

        //public LoginController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        //public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        //{
        //    //var currentUser = _userRepository.

        //    return NotFound();
        //}
    }
}
