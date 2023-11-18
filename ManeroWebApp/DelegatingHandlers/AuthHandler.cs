using ServiceLibrary.Services;

namespace ManeroWebApp.DelegatingHandlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authService = _contextAccessor.HttpContext!.RequestServices.GetRequiredService<IJwtAuthenticationService>();
            var token = await authService.RefreshTokenIfExpired();
            request.Headers.Add("Authorization", "Bearer " + token);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
