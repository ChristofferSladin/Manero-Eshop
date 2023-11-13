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


        public async Task<IActionResult> ProductCardsPartial(string sort)
        {

            var orderBy = "";
            var orderDirection = "";
            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = sort.Split(",")[0];
                orderDirection = sort.Split(",")[1];
            }

            var products = await _productService.GetFilteredProductsAsync(null, null, null, orderBy, orderDirection, null);
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

            }).DistinctBy(p => p.ProductName).Where(p => p.IsFeatured)
            .ToList();

            return PartialView("/Views/Shared/Product/_ProductCards.cshtml", productViewModels);
        }

    }
}
