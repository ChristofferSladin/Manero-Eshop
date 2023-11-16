using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public interface IJwtAuthenticationService
    {
        Task<string> RefreshTokenIfExpired();
        Task<bool> GetTokenAsync(string email, string password);
        Task<bool> RefreshTokenAsync();
        Task<bool> RevokeTokenAsync();
    }
}
