using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class WelcomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
