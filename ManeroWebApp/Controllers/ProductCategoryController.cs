using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;

namespace ManeroWebApp.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductService _productService;

        public ProductCategoryController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var genderList = await _productService.GetProductCategoriesAsync("gender");
            var productCategories = await _productService.GetProductCategoriesAsync("category");

            CategoryPageViewModel viewModel = new();
            viewModel.GenderCategories = genderList;
            viewModel.ProductCategories = productCategories;

            return View(viewModel);
        }
        public async Task<IActionResult> GetProductSubCategories(string genderCategory)
        {

            var test = _productService.GetProductSubCategoriesAsync(genderCategory);
            return RedirectToAction("Index", "ProductCategory");
        }
    }
}
