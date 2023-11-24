using DataAccessLibrary.Entities.UserEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using UserAPI.Dtos;

namespace UserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly FavoriteRepository _favoriteRepository;
        private readonly FavoriteProductRepository _favoriteProductRepository;
        private readonly ShoppingCartRepository _shoppingCartRepository;
        private readonly ShoppingCartProductRepository _shoppingCartProductRepository;
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;

        public UserController(FavoriteRepository favoriteRepository, ShoppingCartRepository shoppingCartRepository, ShoppingCartProductRepository shoppingCartProductRepository, ProductRepository productRepository, FavoriteProductRepository favoriteProductRepository, UserRepository userRepository)
        {
            _favoriteRepository = favoriteRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartProductRepository = shoppingCartProductRepository;
            _productRepository = productRepository;
            _favoriteProductRepository = favoriteProductRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieve All favorite products from the database for a particular user
        /// </summary>
        /// <returns>
        /// Product
        /// </returns>
        /// <remarks>
        /// Example end point: GET /favoriteProducts
        /// </remarks>
        /// <response code="200">
        /// Successfully returned list of all favorite products for a user
        /// </response>
        [HttpGet]
        [Route("/favoriteProducts")]
        public async Task<ActionResult<IEnumerable<FavoriteProductDto>>> GetFavoriteProductsByUser(string userId)
        {
            try
            {
                var query = await _favoriteRepository.GetAsync(x => x.Id == userId,
                    query => query.Include(x => x.FavoriteProducts)!
                    .ThenInclude(x => x.Product));

                var favProductsList = new List<FavoriteProductDto>();

                if (query is not null)
                    favProductsList = query.FavoriteProducts!.Select(favProduct => (FavoriteProductDto)favProduct).ToList();

                foreach (var product in favProductsList)
                    product.ShoppingCartId = _shoppingCartRepository.GetAsync(x => x.Id == userId).Result.ShoppingCartId;

                return Ok(favProductsList);
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return Problem();
        }

        /// <summary>
        /// Create entry in the  Shopping Cart Products table in the database
        /// </summary>
        /// <returns>
        /// Shopping cart product Dto
        /// </returns>
        /// <remarks>
        /// Example end point: POST /shoppingCartProduct
        /// </remarks>
        /// <response code="200">
        /// Successfully added entry in the Shopping Cart product table in the database
        /// </response>
        [HttpPost]
        [Route("/createShoppingCartProduct")]
        public async Task<ActionResult> AddShoppingCartEntry(int productId, int shoppingCartId)
        {
            try
            {
                var shoppingCart = await _shoppingCartRepository.GetAsync(x => x.ShoppingCartId == shoppingCartId);
                var product = await _productRepository.GetProductAsync(x => x.ProductId == productId);

                var entry = new ShoppingCartProduct
                {
                    Product = product,
                    ShoppingCart = shoppingCart,
                    ItemQuantity = 1,
                    TotalPriceExcTax = product.PriceExcTax,
                    TotalPriceIncTax = product.PriceIncTax,
                };
                var result = (ShoppingCartProductDto)await _shoppingCartProductRepository.AddAsync(entry);
                return Ok(result);
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return Problem();
        }

        /// <summary>
        /// Create entry in the  FavoriteProduct  table in the database
        /// </summary>
        /// <returns>
        /// FavoriteProduct entry
        /// </returns>
        /// <remarks>
        /// Example end point: POST /createFavoriteProduct
        /// </remarks>
        /// <response code="200">
        /// Successfully added entry in the FavoriteProduct table in the database
        /// </response>
        [HttpPost]
        [Route("/createFavoriteProduct")]
        public async Task<ActionResult> AddProductToWishList(int productId, string userId)
        {
            try
            {
                var favorite = await _favoriteRepository.GetAsync(x => x.Id == userId);
                var product = await _productRepository.GetProductAsync(x => x.ProductId == productId);

                if (favorite != null && product != null)
                {
                    var entry = new FavoriteProduct
                    {
                        Product = product,
                        Favorite = favorite,
                    };

                    return Ok((FavoriteDto)await _favoriteProductRepository.AddAsync(entry));
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return Problem();
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

        /// <summary>
        /// Delete product from the wish list of the given user
        /// </summary>
        /// <returns>
        /// Boolean (True/False)
        /// </returns>
        /// <remarks>
        /// Example end point: POST /wishList/removeProduct?productId={id}&&userId={userId}
        /// </remarks>
        /// <response code="200">
        /// Deleted successfully
        /// </response>
        [HttpPost]
        [Route("/wishList/removeProduct")]
        public async Task<IActionResult> RemoveProductFromFavoriteProductAsync(int productId, string userId)
        {
            try
            {
                var favoriteId = _favoriteRepository.GetAsync(x => x.Id == userId).Result.FavoriteId;
                var entry = await _favoriteProductRepository.GetAsync(x => x.ProductId == productId, x => x.FavoriteId == favoriteId);
                if (entry is null)
                    return NotFound("There is no such entry exists");

                var result = await _favoriteProductRepository.DeleteAsync(entry);
                if (result)
                    return Ok($"Deleted successfully");
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return Problem();
        }
    }
}
