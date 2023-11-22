using ServiceLibrary.Services;

namespace ManeroWebApp.DelegatingHandlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IJwtAuthenticationService>().RenewTokenIfExpiredAsync();
            request.Headers.Add("Authorization", "Bearer " + _httpContextAccessor.HttpContext.Request.Cookies["Token"]);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
