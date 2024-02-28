using Microsoft.AspNetCore.Identity;

namespace BroadcastSocialMedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public ICollection<Broadcast> Broadcasts { get; set; }

        //varför skriver man inte new ICollection<ApplicationUser>(); ?
        public ICollection<ApplicationUser> ListeningTo { get; set; } = new List<ApplicationUser>();

        public string ProfileImageFilenameGUID { get; set; }
    }
}
