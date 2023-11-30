using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManeroWebApp.Controllers
{
    public class NavigationController : Controller
    {
        public IActionResult NavigationPartial()
        {
            return PartialView("/Views/Shared/Header/_Navigation.cshtml");
        }
    }
}
