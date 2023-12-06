using ServiceLibrary.Models;

namespace ManeroWebApp.Models
{
    public class UserProfileViewModel
    {
        public string? ProfileImage { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public static implicit operator UserProfileViewModel(UserProfile profile)
        {
            return new UserProfileViewModel
            {
                ProfileImage = profile.ProfileImage,
                FirstName = profile.FirstName,
                LastName = profile.LastName
            };
        }
    }
}
