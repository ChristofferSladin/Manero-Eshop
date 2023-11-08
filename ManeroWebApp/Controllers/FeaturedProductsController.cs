using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;

namespace ManeroWebApp.Controllers
{

    public class FeaturedProductsController : Controller
    {
        private readonly IProductService _productService;

        public FeaturedProductsController(IProductService productService)
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
        public IActionResult BarMenuPartial()
        {
            return PartialView("/Views/Shared/_BarMenu.cshtml");
        }
    }
}
