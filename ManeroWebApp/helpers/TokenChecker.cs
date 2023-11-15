using System.IdentityModel.Tokens.Jwt;

namespace ManeroWebApp.helpers
{
    public static class TokenChecker
    {
        public static bool IsTokenExpired(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;
            return token == null || token.ValidTo < DateTime.UtcNow;
        }

    }
}
