using Castle.Core.Logging;
using ManeroWebApp.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceLibrary.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace ManeroWebApp.Tests.ManeroApp;

public class LoginTests
{
    [Fact]
    public async Task Successful_Login_Redirects_To_HomePage()
    {

    }
}
