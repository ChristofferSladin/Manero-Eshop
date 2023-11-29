using DataAccessLibrary.Repositories;
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
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartProductRepository _shoppingCartProductRepository;

        public ShoppingCartController(ShoppingCartProductRepository shoppingCartProductRepository)
        {
            _shoppingCartProductRepository = shoppingCartProductRepository;
        }

        /// <summary>
        /// Retrieve All products in shopping cart for specific user
        /// </summary>
        /// <returns>
        /// Product belonging to user's cart
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/cart/products
        /// </remarks>
        /// <response code="200">
        /// Successfully returned list of all products in user's cart
        /// </response>
        [HttpGet]
        [Route("/user/cart/products")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartProductDto>> GetShoppingCartProductsByUserAsync()
        {
            try
            {
                
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _shoppingCartProductRepository.GetShoppingCartProductsAsync(user);
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
        /// Add product to user's cart
        /// </summary>
        /// <returns>
        /// Product bool
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/cart/add
        /// </remarks>
        /// <response code="200">
        /// Successfully returned true
        /// </response>
        [HttpPost]
        [Route("/user/cart/add")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartProductDto>> AddProductToShoppingCartAsync(int quantity, string productNumber)
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _shoppingCartProductRepository.AddProductAndQuantityToCart(user, quantity, productNumber);
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
        /// Remove product from user's cart
        /// </summary>
        /// <returns>
        /// Product bool
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/cart/remove
        /// </remarks>
        /// <response code="200">
        /// Successfully returned true
        /// </response>
        [HttpPost]
        [Route("/user/cart/remove")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartProductDto>> RemoveProductFromShoppingCartAsync(string productNumber)
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _shoppingCartProductRepository.RemoveProductFromCart(user, productNumber);
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
        /// Increment product in user's cart
        /// </summary>
        /// <returns>
        /// Product bool
        /// </returns>
        /// <remarks>
        /// Example end point: GET /user/cart/increment
        /// </remarks>
        /// <response code="200">
        /// Successfully returned true
        /// </response>
        [HttpPost]
        [Route("/user/cart/increment")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartProductDto>> IncrementProductInShoppingCartAsync(Increment increment, string productNumber)
        {
            try
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null!)
                {
                    var query = await _shoppingCartProductRepository.IncrementProductInCart(user, increment, productNumber);
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
