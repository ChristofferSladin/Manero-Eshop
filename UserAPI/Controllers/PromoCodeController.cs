using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using UserAPI.Dtos;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly UserPromoCodeRepository _userPromoCodeRepository;
        public PromoCodeController(UserPromoCodeRepository userPromoCodeRepository)
        {
            _userPromoCodeRepository = userPromoCodeRepository;
        }

        [HttpGet]
        [Route("/user/promo-code")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PromoCodeDto>>> GetPromoCodesByUserAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

                if (userId != null!)
                {
                    var query = await _userPromoCodeRepository.GetAllByUserAsync(userId);
                    var promoCodes = query.Select(product => (PromoCodeDto)product).ToList();

                    return Ok(promoCodes);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return NotFound();
            }
        }
    }
}
