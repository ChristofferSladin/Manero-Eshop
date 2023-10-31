using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ProductAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ProductController : ControllerBase
    {
        public readonly ProductRepository _productRepository;
        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieve ONE Product from the database (only Product)
        /// </summary>
        /// <returns>
        /// A Product
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Product
        /// </remarks>
        /// <response code="200">
        /// Successfully returned ONE Product
        /// </response>
        [HttpGet]
        [Route("/product")]
        public async Task<ActionResult<Product>> GetProductAsync(int? id = null, string productNumber = null!, string productName = null!)
        {
            var providedFilterCount = 0;
            if (id.HasValue) providedFilterCount++;
            if (!string.IsNullOrEmpty(productNumber)) providedFilterCount++;
            if (!string.IsNullOrEmpty(productName)) providedFilterCount++;

            if (providedFilterCount != 1)
            {
                return BadRequest("Provide only one filter option: id, productNumber, or productName.");
            }

            Expression<Func<Product, bool>> filterBy = null!;

            if (id.HasValue)
            {
                filterBy = product => product.ProductId == id;
            }
            else if (!string.IsNullOrEmpty(productNumber))
            {
                filterBy = product => product.ProductNumber == productNumber;
            }
            else if (!string.IsNullOrEmpty(productName))
            {
                filterBy = product => product.ProductName == productName;
            }

            var product = await _productRepository.GetProductAsync(filterBy);

            if (product == null!)
            {
                return BadRequest("Products not found");
            }

            return Ok(product);
        }

        /// <summary>
        /// Retrieve ONE Product with Reviews from the database (Product and Reviews)
        /// </summary>
        /// <returns>
        /// A Product with Reviews
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Product/Reviews
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a product with reviews
        /// </response>
        [HttpGet]
        [Route("/product/reviews")]
        public async Task<ActionResult<Product>> GetProductWithReviewsAsync(int? id = null, string productNumber = null!, string productName = null!)
        {
            var providedFilterCount = 0;
            if (id.HasValue) providedFilterCount++;
            if (!string.IsNullOrEmpty(productNumber)) providedFilterCount++;
            if (!string.IsNullOrEmpty(productName)) providedFilterCount++;

            if (providedFilterCount != 1)
            {
                return BadRequest("Provide only one filter option: id, productNumber, or productName.");
            }

            Expression<Func<Product, bool>> filterBy = null!;

            if (id.HasValue)
            {
                filterBy = product => product.ProductId == id;
            }
            else if (!string.IsNullOrEmpty(productNumber))
            {
                filterBy = product => product.ProductNumber == productNumber;
            }
            else if (!string.IsNullOrEmpty(productName))
            {
                filterBy = product => product.ProductName == productName;
            }

            var product = await _productRepository.GetProductWithReviewsAsync(filterBy);

            if (product == null!)
            {
                return BadRequest("Products not found");
            }

            return Ok(product);
        }

        /// <summary>
        /// Retrieve ALL Products from the database (only Products)
        /// </summary>
        /// <returns>
        /// A full list of ALL Products
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Products
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of ALL Products
        /// </response>
        [HttpGet]
        [Route("/products")]
        public async Task<ActionResult<List<Product>>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found");
            }
            return Ok(products);
        }

        /// <summary>
        /// Retrieve ALL Products with Reviews from the database (Products and Reviews)
        /// </summary>
        /// <returns>
        /// A full list of ALL Products
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Products/Reviews
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of ALL Products
        /// </response>
        [HttpGet]
        [Route("/products/reviews")]
        public async Task<ActionResult<List<Product>>> GetAllProductsWithReviewsAsync()
        {
            var products = await _productRepository.GetAllProductsWithReviewsAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found");
            }
            return Ok(products);
        }
    }
}
