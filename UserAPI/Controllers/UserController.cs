﻿using DataAccessLibrary.Entities.UserEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using UserAPI.Dtos;

namespace UserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieve user profile by id
        /// </summary>
        /// <returns>
        /// UserProfile
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/profile{id}
        /// This returns users firstName, lastName and profilePicture
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a user profile
        /// </response>
        [HttpGet]
        [Route("/user/profile")]
        public async Task<ActionResult<UserProfile>> GetUserAsync(string id)
        {
            Expression<Func<UserProfile, bool>> expression = user => user.Id == id;
            var user = await _userRepository.GetUserByIdAsync(expression);

            if (user == null!)
            {
                return BadRequest("user not found");
            }

            var userProfile = new UserProfile
            {
                ProfileImage = user.ProfileImage,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return Ok(userProfile);
        }
        [HttpGet]
        [Route("/user")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var user = await _userRepository.GetAsync(x => x.Id == userId);

                    return Ok(user);
                }
                return Unauthorized();
            }
            catch (Exception e) { Debug.WriteLine(e.Message); return Unauthorized(); }

        }

    }
}
