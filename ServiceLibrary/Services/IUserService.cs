using ServiceLibrary.Models;
using System.Security.Claims;

namespace ServiceLibrary.Services;

public interface IUserService
{
    Task<UserProfile> GetUserProfileAsync(string id);
    Task<bool> RemoveProductFromWishListAsync(int productId, string userId);
    Task<bool> CheckApiStatusAsync();
    Task<User> GetIdentityUser();
}
