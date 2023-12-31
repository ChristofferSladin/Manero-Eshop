﻿using ManeroWebApp.Models;
using ManeroWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductControllerService _productControllerService;
        public ProductController(IProductControllerService productControllerService)
        {
            _productControllerService = productControllerService;
        }

        public IActionResult Index(string productNumber)
        {
            return View((object)productNumber);
        }
        public async Task<IActionResult> ProductLikeButtonPartial(string productNumber)
        {
            var userLiked = new LikeViewModel { ProductNumber = productNumber };
            var userLikes = await _productControllerService.GetFavoritesForUserAsync();
            userLiked.Liked = userLikes.Any(l => l.ProductNumber == productNumber);

            return PartialView("/Views/Shared/Product/_ProductLikeButton.cshtml", userLiked);
        }
        public async Task<IActionResult> AddToFavorites(string productNumber, bool liked)
        {
            if (liked)
            {
                await _productControllerService.RemoveProductToFavoriteForUserAsync(productNumber);
            }
            else
            {
                await _productControllerService.AddProductToFavoriteForUserAsync(productNumber);
            }
            return RedirectToAction("Index", "Product", new { productNumber });
        }
        public async Task<IActionResult> AddItemToShoppingCart(int itemQuantity, string productNumber)
        {

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                await _productControllerService.AddProductToShoppingCartForUserAsync(itemQuantity, productNumber);
            }
            else
            {
                _productControllerService.AddProductToShoppingCartForGuest(itemQuantity, productNumber);
            }

            return RedirectToAction("Index", "Product", new { productNumber });
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
            var productViewModels = await _productControllerService.GetProductsWithReviewsAsync();

            return PartialView("/Views/Shared/Product/_ProductCards.cshtml", productViewModels);
        }

        public async Task<IActionResult> ProductCardPartial(string productNumber)
        {

            var productViewModel = await _productControllerService.GetProductAsync(productNumber);
            var reviewData = await _productControllerService.GetReviewDataAsync(productViewModel.ProductName);
            ViewBag.ReviewCount = reviewData.ReviewCount;

            return PartialView("/Views/Shared/Product/_ProductCard.cshtml", productViewModel);
        }

        public async Task<IActionResult> ProductRatingPartial(string productName)
        {
            var ratingViewModel = await _productControllerService.GetReviewDataAsync(productName);

            return PartialView("/Views/Shared/Product/_ProductRating.cshtml", ratingViewModel);
        }

        public async Task<IActionResult> ProductSizesPartial(string productName, string productNumber)
        {
            var sizeViewModel = await _productControllerService.GetProductSizesAsync(productName, productNumber);
            ViewData["productNumber"] = productNumber;

            return PartialView("/Views/Shared/Product/_ProductSizes.cshtml", sizeViewModel);
        }

        public async Task<IActionResult> ProductColorsPartial(string productName, string productNumber, string size)
        {
            var colorViewModel = await _productControllerService.GetProductColorsAsync(productName, size);
            ViewData["productNumber"] = productNumber;

            return PartialView("/Views/Shared/Product/_ProductColors.cshtml", colorViewModel);
        }

        public async Task<IActionResult> ProductReviewsPartial(string productName, int? take)
        {
            var reviewsViewModel = await _productControllerService.GetReviewsForProductAsync(productName, take);

            return PartialView("/Views/Shared/Product/_ProductReviews.cshtml", reviewsViewModel);
        }
        public async Task<IActionResult> ProductReviewsCountPartial(string productName)
        {
            var reviewsViewModel = await _productControllerService.GetReviewDataAsync(productName);

            return PartialView("/Views/Shared/Product/_ProductReviewsCount.cshtml", reviewsViewModel);
        }
    }
}
