using DataAccessLibrary.Entities.UserEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using UserAPI.Dtos;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoCodeController : ControllerBase
    {
        private readonly UserPromoCodeRepository _userPromoCodeRepository;
        private readonly PromoCodeRepository _promoCodeRepository;
        public PromoCodeController(UserPromoCodeRepository userPromoCodeRepository, PromoCodeRepository promoCodeRepository)
        {
            _userPromoCodeRepository = userPromoCodeRepository;
            _promoCodeRepository = promoCodeRepository;
        }
        /// <summary>
        /// Retrieve All Promo Codes for the specific user
        /// </summary>
        /// <returns>
        /// List of Promo Codes
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/promo-code
        /// </remarks>
        /// <response code="200">
        /// Successfully returned list of all user's Promo Codes
        /// </response>
        [HttpGet]
        [Route("/user/promo-code")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PromoCodeDto>>> LinkPromoCodeToUserAsync()
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

        /// <summary>
        /// Links Promo Code for the specific user
        /// </summary>
        /// <returns>
        /// Reponse message of request result
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/link/promo-code
        /// </remarks>
        /// <response code="200">
        /// Promo Code is linked successfully
        /// </response>
        [HttpPost]
        [Route("/user/link/promo-code")]
        [Authorize]
        public async Task<IActionResult> LinkPromoCodeToUserAsync(string promoCodeText)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

                if (userId is null)
                    return Unauthorized();

                var currentPromoCodesForUser = await _userPromoCodeRepository
                    .GetAllByUserAsync(userId);

                var promoCode = await _promoCodeRepository
                    .GetAsync(x => x.PromoCodeText.ToLower() == promoCodeText.ToLower());

                if(promoCode is null)
                    return NotFound("No such promo code exists.");

                if (currentPromoCodesForUser.Any(x => x.PromoCodeText == promoCodeText))
                    return Conflict("Promo code is already registered.");

                await _userPromoCodeRepository.AddAsync(new UserPromoCode
                {
                    Id = userId,
                    PromoCodeId = promoCode.PromoCodeId
                });

                return Ok("Promo Code is linked successfully.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Problem("Something went wrong");
            }
        }
    }
}
