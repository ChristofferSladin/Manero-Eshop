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
        public IActionResult FeaturedProducts()
        {
            return View();
        }
      
    }
}