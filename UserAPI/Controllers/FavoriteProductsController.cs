﻿using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using UserAPI.Dtos;

namespace UserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class FavoriteProductsController : ControllerBase
    {
        private readonly FavoriteProductRepository _favoriteProductRepository;

        public FavoriteProductsController(FavoriteProductRepository favoriteProductRepository)
        {
            _favoriteProductRepository = favoriteProductRepository;
        }

        /// <summary>
        /// Retrieve All products in favorites for specific user
        /// </summary>
        /// <returns>
        /// Product belonging to user's favorites
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/favorites/products
        /// </remarks>
        /// <response code="200">
        /// Successfully returned list of all user's favorite products
        /// </response>
        [HttpGet]
        [Route("/user/favorite/products")]
        [Authorize]
        public async Task<ActionResult<FavoriteProductDto>> GetFavoriteProductsByUserAsync()
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _favoriteProductRepository.GetFavoriteProductsAsync(user);
                    return Ok(query);
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
        /// Add product to user's favorite
        /// </summary>
        /// <returns>
        /// Product bool
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/favorite/add
        /// </remarks>
        /// <response code="200">
        /// Successfully returned true
        /// </response>
        [HttpPost]
        [Route("/user/favorite/add")]
        [Authorize]
        public async Task<ActionResult<FavoriteProductDto>> AddProductToFavoritesAsync(string productNumber)
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _favoriteProductRepository.AddProductToFavorites(user, productNumber);
                    return Ok(query);
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
        /// Remove product from user's favorite
        /// </summary>
        /// <returns>
        /// Product bool
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/favorite/remove
        /// </remarks>
        /// <response code="200">
        /// Successfully returned true
        /// </response>
        [HttpPost]
        [Route("/user/favorite/remove")]
        [Authorize]
        public async Task<ActionResult<FavoriteProductDto>> RemoveProductToFavoritesAsync(string productNumber)
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _favoriteProductRepository.RemoveProductToFavorites(user, productNumber);
                    return Ok(query);
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
