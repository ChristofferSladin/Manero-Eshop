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

        public IActionResult Index()
        {
            return View();
        }
    }
}
