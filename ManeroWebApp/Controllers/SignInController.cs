using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
    }
}
