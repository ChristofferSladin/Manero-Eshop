using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult SignIn()
        {
            return View("~/Views/SignInPage/SignIn.cshtml");
        }

        public IActionResult ForgotPassword()
        {
            return View("~/Views/SignInPage/PasswordForgot.cshtml");
        }
    }
}
