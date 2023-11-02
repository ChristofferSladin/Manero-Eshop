using System.ComponentModel.DataAnnotations;
using ManeroWebApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> ProductCardPartial(int productId = 1)
        {
            var product = await _productService.GetProductWithReviewsAsync(productId);
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
                SalePrice = product.SalePrice,
                IsOnSale = product.IsOnSale,
                IsFeatured = product.IsFeatured,
                Rating = product.Rating,
                ImageUrl = product.ImageUrl,
                Reviews = (product.Reviews ?? null!).Select(r => new ReviewViewModel
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Created =r.Created,
                    Content = r.Content,
                    Title = r.Title,
                    ProductId =r.ProductId,
                    Id = r.Id
                }).ToList()
            };

            return PartialView("/Views/Shared/Product/_ProductCard.cshtml", productViewModel);
        }
        public async Task<IActionResult> ProductSizesPartial(string productName = "Denim Jacket")
        {
            var product = await _productService.GetFilteredProductsAsync(null, null, null, "size", "asc", productName);
            var sizeViewModel = product.Select(p => new SizeViewModel
            {
                ProductName = p.ProductName,
                Size = p.Size,

            }).ToList();
            sizeViewModel = sizeViewModel.DistinctBy(c => c.Size).ToList();

            return PartialView("/Views/Shared/Product/_ProductSizes.cshtml", sizeViewModel);
        }
        public async Task<IActionResult> ProductColorsPartial(string productName = "Denim Jacket")
        {
            var product = await _productService.GetFilteredProductsAsync(null, null, null, "color", "asc", productName);
            var colorViewModel = product.Select(p => new ColorViewModel
            {
                ProductName = p.ProductName,
                Color = p.Color,

            }).ToList();
            colorViewModel = colorViewModel.DistinctBy(c => c.Color).ToList();

            return PartialView("/Views/Shared/Product/_ProductColors.cshtml", colorViewModel);
        }
    }
}
