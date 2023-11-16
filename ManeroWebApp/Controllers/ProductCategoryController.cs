using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.Collections.Immutable;

namespace ManeroWebApp.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly IProductService _productService;

        public ProductCategoryController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(string genderCategory)
        {

            var genderCategories = _productService.GetGenderCategoriesAsync().Result;
            List<Category> query;
            CategoryPageViewModel viewModel = new();
            viewModel.GenderCategories = genderCategories;

            genderCategory ??= genderCategories.FirstOrDefault()!;

            query = await _productService.GetProductSubCategoriesAsync(genderCategory);
            viewModel.ProductCategories = query.Select(x => new Category
                {
                    Id = x.Id,
                    ImgUrl = x.ImgUrl,
                    CategoryName = x.CategoryName,
                }).ToList();


            string script = $"var element = document.getElementById('{genderCategory}'); " +
                $"console.log(element);" +
                $"element.classList.add('active')";
            
            ViewData["script"] = script;
                            

            return View(viewModel);
        }
    }
}
