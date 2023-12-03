using ManeroWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Services;
using System.Diagnostics;

namespace ManeroWebApp.Controllers
{
    [Authorize]
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

                if (status == "current" && !promoCodes.Any())
                    return View("PromoCodes/_AddPromoCodePartial");

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

        public async Task<IActionResult> AddPromoCode(AddPromoCodeViewModel viewModel)
        {
            try
            {
                if (viewModel.Voucher is not null)
                {
                    var response = await _promoCodeService
                        .LinkPromoCodeToUserAsync(viewModel.Voucher!);
                    
                    TempData["info"] = response;
                }
            }
            catch (Exception e) { Debug.WriteLine(e.Message); }
            return View("PromoCodes/_AddPromoCodePartial");
        }
    }
}
