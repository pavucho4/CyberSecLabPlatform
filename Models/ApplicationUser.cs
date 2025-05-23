using Microsoft.AspNetCore.Identity;

namespace CyberSecLabPlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
