namespace UserAPI.Models
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public required string RefreshToken { get; set; }
    }
}
