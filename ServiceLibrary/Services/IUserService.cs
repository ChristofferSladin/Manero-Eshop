using ServiceLibrary.Models;
using System.Security.Claims;

namespace ServiceLibrary.Services;

public interface IUserService
{
    Task<UserProfile> GetUserProfileAsync(string id);
}
