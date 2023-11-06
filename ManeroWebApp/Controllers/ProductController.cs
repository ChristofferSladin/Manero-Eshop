using System.ComponentModel.DataAnnotations;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

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
        public IActionResult FeaturedProduct()
        {
            return View();
        }
        public async Task<IActionResult> AddProductToWishList(int productId)
        {
            var result = await _userService.AddProductToWishList(productId, "daf64035-b7fb-4bd1-b863-e19cab55699f");
            if(result!=null)
            {
                TempData["success"] = $"Product added to wish list.";
                return RedirectToAction("FeaturedProduct", "Product");
            }
            TempData["error"] = $"Somthing went wrong.";
            return RedirectToAction("FeaturedProduct", "Product");
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
                SalePrice = product.SalePrice,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl
            }).ToList();

            return PartialView("/Views/Shared/Product/_ProductCards.cshtml", productViewModels);
        }
    }
}