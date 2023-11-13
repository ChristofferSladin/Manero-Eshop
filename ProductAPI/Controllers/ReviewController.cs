using System.Linq.Expressions;
using DataAccessLibrary.Entities.ProductEntities;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepository;
        public ReviewController(ReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        /// <summary>
        /// Retrieve FILTERED or ALL Reviews from the database filtered on chosen criterias below (Reviews)
        /// </summary>
        /// <returns>
        /// A full list of FILTERED or ALL Reviews
        /// </returns>
        /// <remarks>
        /// Example end point: GET /Reviews/filter
        /// </remarks>
        /// <response code="200">
        /// Successfully returned a list of FILTERED or ALL Reviews
        /// </response>
        [HttpGet]
        [Route("/reviews/filter")]
        public async Task<ActionResult<List<Review>>> GetFilteredReviewsAsync(int? page, int? take, string filterByName = null!, string? orderByField = null!, string orderDirection = null!)
        {
            if (page <= 0 && take <= 0) { return BadRequest($"Invalid page & take, value \"{page}\" & value \"{take}\" is not allowed."); }
            if (page <= 0) { return BadRequest($"Invalid page, value \"{page}\" is not allowed."); }
            if (take <= 0) { return BadRequest($"Invalid take, value \"{take}\" is not allowed."); }

            var skip = (page - 1) * take;
            var _skip = skip ?? 0;
            var _take = take ?? 0;
    
            Expression<Func<Review, bool>> _filterByName = null!;

            if (!string.IsNullOrEmpty(filterByName))
            {
                _filterByName = review => review.ProductName.ToLower() == filterByName.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(orderDirection) && orderDirection.ToLower() != "asc" && orderDirection.ToLower() != "desc")
            {
                return BadRequest("Invalid order direction use: asc, desc, or leave empty.");
            }

            Expression<Func<Review, object>> _orderByField = null!;

            if (orderByField != null)
            {
                var propertyInfo = typeof(Review).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, orderByField, StringComparison.OrdinalIgnoreCase));

                if (propertyInfo == null) return BadRequest("Invalid property, the orderByField name you provided does not match any fields in the product.");

                var param = Expression.Parameter(typeof(Review), "review");
                Expression propertyAccess = Expression.Property(param, propertyInfo);

                if (propertyInfo.PropertyType == typeof(decimal))
                {
                    _orderByField = Expression.Lambda<Func<Review, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    _orderByField = Expression.Lambda<Func<Review, object>>(Expression.Convert(propertyAccess, typeof(object)), param);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    _orderByField = Expression.Lambda<Func<Review, object>>(propertyAccess, param);
                }
            }

            var reviews = await _reviewRepository.GetFilteredReviewsAsync(_skip, _take, _filterByName, _orderByField, orderDirection);

            if (reviews.Count == 0 && !string.IsNullOrEmpty(filterByName)) { return NotFound("Invalid product name in reviews, the review does not exist."); }
            if (reviews.Count == 0) { return NotFound("Invalid page, page does not exist or has no reviews."); }

            return Ok(reviews);
        }
    }
}
