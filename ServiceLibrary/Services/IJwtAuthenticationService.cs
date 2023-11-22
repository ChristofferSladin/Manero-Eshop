﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public interface IJwtAuthenticationService
    {
        Task<bool> RenewTokenIfExpiredAsync();
        Task<bool> GetTokenAsync(string email, string password);
        Task<bool> RevokeTokenAsync();
    }
}
