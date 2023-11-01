using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult FeaturedProduct()
        {
            return View();
        }
    }
}
