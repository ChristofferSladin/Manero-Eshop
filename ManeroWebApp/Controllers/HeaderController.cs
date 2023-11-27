using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ManeroWebApp.Controllers
{
    public class HeaderController : Controller
    {
        public IActionResult HeaderPartial(string title, string page)
        {
            PageTitleViewModel viewModel = new()
            {
                Title = title,
                Path = page
            };
            return PartialView("/Views/Shared/Header/_Header.cshtml", viewModel);
        }
    }
}
