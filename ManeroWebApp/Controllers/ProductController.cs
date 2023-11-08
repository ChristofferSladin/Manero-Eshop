using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Drawing;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        public IActionResult Index(string productNumber)
        {
            return View((object)productNumber);
        }
        public async Task<IActionResult> ProductCardsPartial()
        {
            var products = await _productService.GetProductsWithReviewsAsync();
            var productViewModels = products.Select(product => new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category,
                Type = product.Type,
                Size = product.Size,
                QuantityInStock = product.QuantityInStock,
                Color = product.Color,
                PriceExcTax = product.PriceExcTax,
                PriceIncTax = product.PriceIncTax,
                SalePricePercentage = product.SalePricePercentage,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl
            }).ToList();

            return PartialView("/Views/Shared/Product/_ProductCards.cshtml", productViewModels);
        }
        public async Task<IActionResult> ProductCardPartial(string productNumber)
        {
            var product = await _productService.GetProductAsync(productNumber);
            var productViewModel = new ProductViewModel
            {
                ProductId = product.ProductId,
                ProductNumber = product.ProductNumber,
                ProductName = product.ProductName,
                Description = product.Description,
                Category = product.Category,
                Type = product.Type,
                Size = product.Size,
                QuantityInStock = product.QuantityInStock,
                Color = product.Color,
                PriceExcTax = product.PriceExcTax,
                PriceIncTax = product.PriceIncTax,
                SalePricePercentage = product.SalePricePercentage,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl,
            };

            return PartialView("/Views/Shared/Product/_ProductCard.cshtml", productViewModel);
        }
        public async Task<IActionResult> ProductRatingPartial(string productName)
        {
            var products = await _productService.GetFilteredProductsWithReviewsAsync(null, null, null, null, null, productName);
            var rating = 0.0M;
            var reviewCount = 0;
            foreach (var p in products)
            {
                if (p.Reviews != null)
                {
                    foreach (var r in p.Reviews)
                    {
                        rating += r.Rating;
                        reviewCount++;
                    }
                }
            }

            rating /= reviewCount;

            var ratingViewModel = new RatingViewModel
            {
                Rating = rating,
                ReviewCount = reviewCount,
            };

            return PartialView("/Views/Shared/Product/_ProductRating.cshtml", ratingViewModel);
        }
        public async Task<IActionResult> ProductSizesPartial(string productName, string productNumber)
        {
            var products = await _productService.GetFilteredProductsAsync(null, null, null, "size", "asc", productName);
            var sizeViewModel = products.Select(p => new SizeViewModel
            {
                ProductName = p.ProductName,
                ProductNumber = p.ProductNumber!,
                Size = p.Size,
            }).ToList();

            var sizes = new[] { "XXS", "XS", "S", "M", "L", "X", "XL", "XXL", "XXXL", "XXXXL" };
            sizeViewModel = sizeViewModel
                .OrderBy(s => sizes.Contains(s.Size) ? "0" : "1")
                .ThenBy(s => Array.IndexOf(sizes, s.Size))
                .ThenBy(s => s.Size).GroupBy(c => c.Size)
                .Select(group => group.FirstOrDefault(c => c.ProductNumber == productNumber) ?? group.First())
                .ToList();

            ViewData["productNumber"] = productNumber;

            return PartialView("/Views/Shared/Product/_ProductSizes.cshtml", sizeViewModel);
        }
        public async Task<IActionResult> ProductColorsPartial(string productName, string productNumber, string size)
        {
            var products = await _productService.GetFilteredProductsAsync(null, null, null, "size", "asc", productName);
            var colorViewModel = products.Where(p => p.Size == size).Select(p => new ColorViewModel
            {
                ProductName = p.ProductName,
                ProductNumber = p.ProductNumber!,
                Color = p.Color,

            }).ToList();

            ViewData["productNumber"] = productNumber;

            return PartialView("/Views/Shared/Product/_ProductColors.cshtml", colorViewModel);
        }
        public async Task<IActionResult> ProductReviewsPartial(string productName)
        {
            var products = await _productService.GetFilteredProductsWithReviewsAsync(null, null, null, null, null, productName);
            var reviewsViewModel = products
                .Where(p => p.Reviews != null)
                .SelectMany(p => p.Reviews!, (_, review) => new
                {
                    Review = review,
                    UserProfile = _userService.GetUserProfileAsync(review.Id).Result
                })
                .OrderByDescending(r => r.Review.Created)
                .Take(5)
                .Select(combined => new ReviewViewModel
                {
                    ReviewId = combined.Review.ReviewId,
                    Rating = combined.Review.Rating,
                    Created = combined.Review.Created,
                    Content = combined.Review.Content,
                    Title = combined.Review.Title,
                    ProductId = combined.Review.ProductId,
                    Id = combined.Review.Id,
                    FirstName = combined.UserProfile.FirstName,
                    LastName = combined.UserProfile.LastName,
                    ProfileImage = combined.UserProfile.ProfileImage,
                })
                .ToList();
            return PartialView("/Views/Shared/Product/_ProductReviews.cshtml", reviewsViewModel);
        }
    }
}
