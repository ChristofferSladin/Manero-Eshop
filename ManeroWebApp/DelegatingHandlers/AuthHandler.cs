using ServiceLibrary.Services;

namespace ManeroWebApp.DelegatingHandlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthService _jwtAuthService;
        public AuthHandler(IHttpContextAccessor httpContextAccessor, IJwtAuthService jwtAuthService)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthService = jwtAuthService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _jwtAuthService.RenewTokenIfExpiredAsync();
            request.Headers.Add("Authorization", "Bearer " + _httpContextAccessor.HttpContext!.Request.Cookies["Token"]);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
