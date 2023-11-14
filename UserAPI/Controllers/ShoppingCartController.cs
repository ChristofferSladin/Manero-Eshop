using System.Diagnostics;
using DataAccessLibrary.Entities.UserEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace UserAPI.Controllers
{
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
        //[Authorize]
        public async Task<ActionResult<ShoppingCartProduct>> GetShoppingCartProductsByUserAsync(string user)
        {
            try
            {
                //var user =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null)
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
        //[Authorize]
        public async Task<ActionResult<ShoppingCartProduct>> AddProductToShoppingCartAsync(string user, int quantity, string productNumber)
        {
            try
            {
                //var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                if (user != null)
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
    }
}
