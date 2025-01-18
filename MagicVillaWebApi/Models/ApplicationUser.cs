using Microsoft.AspNetCore.Identity;

namespace MagicVillaWebApi.Models
{
    public class ApplicationUser: IdentityUser
    {
        //Not using currently, if required use it
        //All Identity tables are created
        public string Name { get; set; }
    }
}
