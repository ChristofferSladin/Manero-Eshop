using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceLibrary.Services;
using System.Diagnostics;
using System.Security.Claims;
using ManeroWebApp.Services;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        private readonly IShoppingCartControllerService _shoppingCartControllerService;

        public ProductController(IProductService productService, IUserService userService, IReviewService reviewService, IShoppingCartControllerService shoppingCartControllerService)
        {
            _productService = productService;
            _userService = userService;
            _reviewService = reviewService;
            _shoppingCartControllerService = shoppingCartControllerService;
        }

        public async Task<IActionResult> ShoppingCartPartial()
        {
            var shoppingCart = new List<ShoppingCartViewModel>();
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Subject;
                shoppingCart = await _shoppingCartControllerService.GetShoppingForUserCart(user?.Name!);
            }
            else
            {
                var shoppingCartCookie = Request.Cookies["ShoppingCart"];
                if (shoppingCartCookie != null)
                {
                    shoppingCart = await _shoppingCartControllerService.GetShoppingForGuestCart(shoppingCartCookie);
                }
            }

            return PartialView("/Views/Shared/ShoppingCart/_ShoppingCartPartial.cshtml", shoppingCart);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int itemQuantity, string productNumber)
        {
         
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Subject;
                await _shoppingCartControllerService.AddProductToShoppingCartForUser(user?.Name!, itemQuantity, productNumber);
            }
            else
            {
                var existingShoppingCartCookie = Request.Cookies["ShoppingCart"];
                _shoppingCartControllerService.AddProductToShoppingCartForGuest(Response, existingShoppingCartCookie, itemQuantity, productNumber);
            }

            return RedirectToAction("Index", "Product", new { productNumber });
        }


        public IActionResult Index(string productNumber)
        {
            return View((object)productNumber);
        }
        public IActionResult Reviews(string productName, string productNumber)
        {
            var productViewModel = new ProductViewModel
            {
                ProductName = productName,
                ProductNumber = productNumber
            };
            return View(productViewModel);
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
            var reviews = await _reviewService.GetFilteredReviewsAsync(null, null, null, null, productName);
            var rating = 0.0M;
            var reviewCount = 0;
            foreach (var p in reviews)
            {
                rating += p.Rating;
                reviewCount++;
            }

            if (reviewCount != 0)
            {
                rating /= reviewCount;
            }

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
        public async Task<IActionResult> ProductReviewsPartial(string productName, int? take)
        {

            var reviews = await _reviewService.GetFilteredReviewsAsync(null, take, "created", "desc", productName);

            var reviewsViewModel = reviews.Select(r =>
            {
                var userProfile = _userService.GetUserProfileAsync(r.Id).Result;
                return new ReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    ProductName = r.ProductName,
                    Rating = r.Rating,
                    Created = r.Created,
                    Content = r.Content,
                    Title = r.Title,
                    ProductId = r.ProductId,
                    Id = r.Id,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    ProfileImage = userProfile.ProfileImage,
                };
            }).ToList();

            return PartialView("/Views/Shared/Product/_ProductReviews.cshtml", reviewsViewModel);
        }
        public async Task<IActionResult> ProductReviewsCountPartial(string productName)
        {
            var reviews = await _reviewService.GetFilteredReviewsAsync(null, null, null, null, productName);
            var reviewsViewModel = new ReviewViewModel
            {
                ReviewCount = reviews.Count
            };
            return PartialView("/Views/Shared/Product/_ProductReviewsCount.cshtml", reviewsViewModel);
        }
    }
}
