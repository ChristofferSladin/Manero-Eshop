﻿using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<ActionResult<List<Product>>> GetProductsFilteredAsync(int? page, int? take, string filterByCategory = null!, string orderByField = null!, string orderDirection = null!)
        {
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

            if (!string.IsNullOrWhiteSpace(orderDirection) && orderDirection.ToLower() != "asc" && orderDirection.ToLower() != "desc")
            {
                return BadRequest("Invalid order direction use: asc, desc, or leave empty.");
            }

            Expression<Func<Product, dynamic>> _orderByField = null!;

            if (orderByField != null!)
            {
                var propertyInfo = typeof(Product).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, orderByField, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null) return BadRequest("Invalid property the orderByField name you provided does not match any fields in product.");

                var param = Expression.Parameter(typeof(Product), "product");
                Expression propertyAccess = Expression.Property(param, propertyInfo);
                _orderByField = Expression.Lambda<Func<Product, dynamic>>(propertyAccess, param);
            }
             
            var products = await _productRepository.GetFilteredProductsAsync(_skip, _take, _filterByCategory, _orderByField, orderDirection);

            if (products.Count == 0) { return BadRequest("Invalid page, page does not exist or has no products."); }

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
        public async Task<ActionResult<List<Product>>> GetProductsFilteredWithReviewsAsync(int? page, int? take, string filterByCategory = null!, string orderByField = null!, string orderDirection = null!)
        {
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

            if (!string.IsNullOrWhiteSpace(orderDirection) && orderDirection.ToLower() != "asc" && orderDirection.ToLower() != "desc")
            {
                return BadRequest("Invalid order direction use: asc, desc, or leave empty.");
            }

            Expression<Func<Product, dynamic>> _orderByField = null!;

            if (orderByField != null!)
            {
                var propertyInfo = typeof(Product).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, orderByField, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null) return BadRequest("Invalid property the orderByField name you provided does not match any fields in product.");

                var param = Expression.Parameter(typeof(Product), "product");
                Expression propertyAccess = Expression.Property(param, propertyInfo);
                _orderByField = Expression.Lambda<Func<Product, dynamic>>(propertyAccess, param);
            }

            var products = await _productRepository.GetFilteredProductsWithReviewsAsync(_skip, _take, _filterByCategory, _orderByField, orderDirection);

            if (products.Count == 0) { return BadRequest("Invalid page, page does not exist or has no products."); }

            return Ok(products);
        }
    }
}
