using Microsoft.AspNetCore.Identity.MongoDB;

namespace IdentityService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class IdentityRole : Microsoft.AspNetCore.Identity.MongoDB.IdentityRole
    {

    }
}