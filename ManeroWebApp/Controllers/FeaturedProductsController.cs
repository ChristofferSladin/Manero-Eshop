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
        public async Task<IActionResult> ProductCardsPartial(string filterByProperty = "productName", string orderDirection = "asc")
        {
            //var products = await _productService.GetProductsWithReviewsAsync();
            var product = await _productService.GetFilteredProductsAsync(null, null, null, filterByProperty, orderDirection, null);
            var productViewModels = product.Select(product => new ProductViewModel
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

            }).DistinctBy(p=>p.ProductName)
            .ToList();

            return PartialView("/Views/Shared/Product/_ProductCards.cshtml", productViewModels);
        }
        public IActionResult BarMenuPartial()
        {
            return PartialView("/Views/Shared/_BarMenu.cshtml");
        }

        public async Task<IActionResult> FilterProductPartial(string filterByProperty, string orderDirection)
        {
            var filterProduct = await _productService.GetFilteredProductsAsync(null, null,null, filterByProperty, orderDirection,null );
            return PartialView();
        }
        
    }
}
