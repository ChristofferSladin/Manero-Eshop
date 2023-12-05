using ManeroWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary.Models;
using ServiceLibrary.Services;
using System.Security.Claims;

namespace ManeroWebApp.Controllers
{
    [Authorize(Roles = "Customer")]
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            UserProfileViewModel userProfileViewModel = new();
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userService.GetIdentityUser();
                var userProfile = await _userService.GetUserProfileAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                if (userProfile != null)
                {
                    userProfileViewModel = userProfile;
                    userProfileViewModel.Email = user.Email;
                }
            }

            return View(userProfileViewModel);
        }


    }

}
