using ManeroWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;

namespace ManeroWebApp.Controllers
{
    public class PromoCodesController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodesController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        public async Task<IActionResult> Index(string status = "current")
        {
            try
            {
                var promoCodes = await _promoCodeService.GetPromoCodesByUserAsync(status);
                if (promoCodes is not null)
                {
                    var viewModel = promoCodes.Select(promoCode => (PromoCodeViewModel)promoCode);
                    string script = $"var element = document.getElementById('{status}'); " +
                        $"element.classList.add('active')";

                    ViewData["script"] = script;
                    
                    return View(viewModel);
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }

            return View();
        }
    }
}
