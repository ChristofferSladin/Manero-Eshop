using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductAPI.Dtos;
using System.Linq.Expressions;

namespace ProductAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
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
        [Authorize]
        public async Task<ActionResult<List<Product>>> GetProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();

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
        public async Task<ActionResult<List<Product>>> GetProductsWithReviewsAsync()
        {
            var products = await _productRepository.GetProductsWithReviewsAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found");
            }
            return Ok(products);
        }

        /// <summary>
        /// Retrieve ALL Products ON SALE with Reviews from the database (Products and Reviews)
        /// </summary>
        /// <returns>
        /// A full list of ALL Products ON SALE
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Products/Reviews
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of ALL Products ON SALE
        /// </response>
        [HttpGet]
        [Route("/products/onsale/reviews")]
        public async Task<ActionResult<List<Product>>> GetOnSaleProductsWithReviewsAsync()
        {
            var products = await _productRepository.GetOnSaleProductsWithReviewsAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found");
            }
            return Ok(products);
        }

        /// <summary>
        /// Retrieve ALL FEATURED Products with Reviews from the database (Products and Reviews)
        /// </summary>
        /// <returns>
        /// A full list of ALL FEATURED Products
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Products/Reviews
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of ALL FEATURED Products
        /// </response>
        [HttpGet]
        [Route("/products/featured/reviews")]
        public async Task<ActionResult<List<Product>>> GetFeaturedProductsWithReviewsAsync()
        {
            var products = await _productRepository.GetFeaturedProductsWithReviewsAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found");
            }
            return Ok(products);
        }

        /// <summary>
        /// Retrieve FILTERED or ALL Products from the database filtered on chosen criterias below (Products)
        /// </summary>
        /// <returns>
        /// A full list of FILTERED or ALL Products
        /// </returns>
        /// <remarks>
        /// Example end point: GET /products/reviews/filter
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of FILTERED or ALL Products
        /// </response>
        [HttpGet]
        [Route("/products/filter")]
        public async Task<ActionResult<List<Product>>> GetProductsFilteredAsync(int? page, int? take, string? gender, string filterByName = null!, string filterByCategory = null!, string? orderByField = null!, string orderDirection = null!)
        {
            if (page <= 0 && take <= 0) { return BadRequest($"Invalid page & take, value \"{page}\" & value \"{take}\" is not allowed."); }
            if (page <= 0) { return BadRequest($"Invalid page, value \"{page}\" is not allowed."); }
            if (take <= 0) { return BadRequest($"Invalid take, value \"{take}\" is not allowed."); }

            var skip = (page - 1) * take;
            var _skip = skip ?? 0;
            var _take = take ?? 0;

            Expression<Func<Product, bool>> _filterByCategory = null!;

            if (!string.IsNullOrEmpty(filterByCategory))
            {
                _filterByCategory = product => product.Category != null && product.Category.ToLower() == filterByCategory.ToLower();
            }

            Expression<Func<Product, bool>> _filterByName = null!;

            if (!string.IsNullOrEmpty(filterByName))
            {
                _filterByName = product => product.ProductName.ToLower() == filterByName.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(orderDirection) && orderDirection.ToLower() != "asc" && orderDirection.ToLower() != "desc")
            {
                return BadRequest("Invalid order direction use: asc, desc, or leave empty.");
            }

            Expression<Func<Product, object>> _orderByField = null!;

            if (orderByField != null)
            {
                var propertyInfo = typeof(Product).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, orderByField, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null) return BadRequest("Invalid property, the orderByField name you provided does not match any fields in the product.");

                var param = Expression.Parameter(typeof(Product), "product");
                Expression propertyAccess = Expression.Property(param, propertyInfo);

                if (propertyInfo.PropertyType == typeof(decimal))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(propertyAccess, param);
                }
            }

            Expression<Func<Product, bool>> _gender = null!;

            if (!string.IsNullOrEmpty(gender))
            {
                _gender = product => product.Gender != null && product.Gender.ToLower() == gender.ToLower();
            }

            var products = await _productRepository.GetFilteredProductsAsync(_skip, _take, _filterByName, _filterByCategory, _orderByField, orderDirection, _gender);

            if (products.Count == 0 && !string.IsNullOrEmpty(filterByName)) { return NotFound("Invalid product name, the product does not exist."); }
            if (products.Count == 0 && !string.IsNullOrEmpty(filterByCategory)) { return NotFound("Invalid category name, the category does not exist."); }
            if (products.Count == 0) { return NotFound("Invalid page, page does not exist or has no products."); }

            return Ok(products);
        }
        /// <summary>
        /// Retrieve FILTERED or ALL Products with Reviews from the database filtered on chosen criterias below (Products and Reviews)
        /// </summary>
        /// <returns>
        /// A full list of FILTERED or ALL Products with Reviews
        /// </returns>
        /// <remarks>
        /// Example end point: GET /products/reviews/filter
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of FILTERED or ALL Products
        /// </response>
        [HttpGet]
        [Route("/products/reviews/filter")]
        public async Task<ActionResult<List<Product>>> GetProductsFilteredWithReviewsAsync(int? page, int? take, string filterByName = null!, string filterByCategory = null!, string orderByField = null!, string orderDirection = null!)
        {
            if (page <= 0 && take <= 0) { return BadRequest($"Invalid page & take, value \"{page}\" & value \"{take}\" is not allowed."); }
            if (page <= 0) { return BadRequest($"Invalid page, value \"{page}\" is not allowed."); }
            if (take <= 0) { return BadRequest($"Invalid take, value \"{take}\" is not allowed."); }

            var skip = (page - 1) * take;
            var _skip = skip ?? 0;
            var _take = take ?? 0;

            Expression<Func<Product, bool>> _filterByCategory = null!;

            if (!string.IsNullOrEmpty(filterByCategory))
            {
                _filterByCategory = product => product.Category != null && product.Category.ToLower() == filterByCategory.ToLower();
            }

            Expression<Func<Product, bool>> _filterByName = null!;

            if (!string.IsNullOrEmpty(filterByName))
            {
                _filterByName = product => product.ProductName.ToLower() == filterByName.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(orderDirection) && orderDirection.ToLower() != "asc" && orderDirection.ToLower() != "desc")
            {
                return BadRequest("Invalid order direction use: asc, desc, or leave empty.");
            }

            Expression<Func<Product, object>> _orderByField = null!;

            if (orderByField != null)
            {
                var propertyInfo = typeof(Product).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, orderByField, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null) return BadRequest("Invalid property, the orderByField name you provided does not match any fields in the product.");

                var param = Expression.Parameter(typeof(Product), "product");
                Expression propertyAccess = Expression.Property(param, propertyInfo);

                if (propertyInfo.PropertyType == typeof(decimal))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    _orderByField = Expression.Lambda<Func<Product, object>>(propertyAccess, param);
                }
            }

            var products = await _productRepository.GetFilteredProductsWithReviewsAsync(_skip, _take, _filterByName, _filterByCategory, _orderByField, orderDirection);

            if (products.Count == 0 && !string.IsNullOrEmpty(filterByName)) { return NotFound("Invalid product name, the product does not exist."); }
            if (products.Count == 0 && !string.IsNullOrEmpty(filterByCategory)) { return NotFound("Invalid category name, the category does not exist."); }
            if (products.Count == 0) { return NotFound("Invalid page, page does not exist or has no products."); }

            return Ok(products);
        }

        [HttpGet]
        [Route("/products/genderCategories")]
        public async Task<IActionResult> GetGenderCategories()
        {
            var categories = await _productRepository.GetGenderCategories();
            return Ok(categories);
        }
        [HttpGet]
        [Route("/products/subcategories")]
        public async Task<IActionResult> GetProductSubCategories(string genderCategory)
        {
            var categories = await _productRepository.GetProductSubCategories(genderCategory);
            if (categories is not null)
                return Ok(categories);
            
            return null!;
        }
        [HttpGet]
        [Route("/productsByGenderCategory")]
        public async Task<IActionResult> GetProductByGenderCategory(string genderCategory, string productCategory)
        {
            var products = await _productRepository.GetProductsByGenderCategory(genderCategory, productCategory);

            if(products is not null)
                return Ok(products);
            
            return null!;
        }
    }
}
