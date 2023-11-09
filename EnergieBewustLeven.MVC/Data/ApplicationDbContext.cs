using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EnergieBewustLeven.API.Models.DTO;

namespace EnergieBewustLeven.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EnergieBewustLeven.API.Models.DTO.ApplianceDTO> ApplianceDTO { get; set; } = default!;
    }
}