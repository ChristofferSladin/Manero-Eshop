namespace ManeroWebApp.Services;

public interface ICookieService
{
    bool CreateUserHasSelectedLoginCookie(HttpContext httpContext);
}
