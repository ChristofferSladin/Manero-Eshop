using Microsoft.AspNetCore.Http;
using ManeroWebApp.Services;

namespace ManeroWebApp.Tests
{
    public class CookieServiceTests
    {
        [Fact]
        public void TryCreateUserHasSelectedLoginCookie_ShouldAppendCookie_WhenCookieNotExists()
        {
            var context = new DefaultHttpContext();
            var cookieService = new CookieService();

            bool cookieCreated = cookieService.CreateUserHasSelectedLoginCookie(context);

            Assert.True(cookieCreated);
            var cookieHeaderValue = context.Response.Headers["Set-Cookie"].ToString();
            Assert.Contains("UserHasSelectedLogin", cookieHeaderValue);
        }
    }
}
