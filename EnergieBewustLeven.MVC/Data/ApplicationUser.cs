using Microsoft.AspNetCore.Identity;

namespace EnergieBewustLeven.MVC.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public int Level { get; set; }

    }
}
