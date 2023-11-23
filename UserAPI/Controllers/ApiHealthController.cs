using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Dtos;

namespace UserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ApiHealthController : ControllerBase
    {
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
        [Route("/health")]
        public IActionResult Available()
        {
            var available = true;
            if (available)
            {
                return Ok(available);
            }
            return StatusCode(503, false);
        }
    }
}
