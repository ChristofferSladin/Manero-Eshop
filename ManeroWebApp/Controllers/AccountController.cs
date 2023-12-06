using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn(string? returnUrl = null)
        {
            if(returnUrl == null) 
            {
                returnUrl = "~/Home";
            }
            ViewData["returnUrl"] = returnUrl;
            return View("~/Views/SignInPage/SignIn.cshtml");
        }

        public IActionResult ForgotPassword()
        {
            return View("~/Views/SignInPage/PasswordForgot.cshtml");
        }
    }
}
