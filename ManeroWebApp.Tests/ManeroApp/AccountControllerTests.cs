using ManeroWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeroWebApp.Tests.ManeroApp;

public class AccountControllerTests
{
    [Fact]
    public void SignIn_Without_ReturnUrl_Redirects_To_Home()
    {
        var controller = new AccountController();
        var result = controller.SignIn(null) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("~/Home", result.ViewData["returnUrl"]);
        Assert.Equal("~/Views/SignInPage/SignIn.cshtml", result.ViewName);
    }
}
