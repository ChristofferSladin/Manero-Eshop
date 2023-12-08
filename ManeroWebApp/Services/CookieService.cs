namespace ManeroWebApp.Services
{
    public class CookieService : ICookieService
    {
        public bool CreateUserHasSelectedLoginCookie(HttpContext httpContext)
        {
            var cookie = httpContext.Request.Cookies["UserHasSelectedLogin"];
            if (cookie == null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(100),
                };
                httpContext.Response.Cookies.Append("UserHasSelectedLogin", "true", cookieOptions);
                return true;
            }
            return false;
        }
    }
}
