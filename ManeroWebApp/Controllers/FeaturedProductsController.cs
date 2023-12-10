using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Drawing;

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


        public async Task<IActionResult> ProductCardsPartial(string sort, List<string>? selectedSizes = null)
        {
            var orderBy = string.Empty;
            var orderDirection = string.Empty;
            var gender = string.Empty;
            var color = string.Empty;

            decimal? minPrice = null;
            decimal? maxPrice = null;
            selectedSizes ??= new List<string>();

            if (!string.IsNullOrEmpty(sort))
            {
                orderBy = sort.Split(",")[0];
                orderDirection = sort.Split(",")[1];
            }

            var products = await _productService.SelectFilteredProductsAsync(null, null, null, orderBy, orderDirection, null, gender, color, minPrice, maxPrice, selectedSizes);
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
