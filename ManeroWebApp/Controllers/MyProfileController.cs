using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class MyProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
