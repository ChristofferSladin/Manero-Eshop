using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class OnSaleProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
