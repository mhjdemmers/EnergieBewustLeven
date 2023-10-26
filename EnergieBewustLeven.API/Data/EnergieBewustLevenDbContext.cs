using Microsoft.EntityFrameworkCore;
using EnergieBewustLeven.API.Models.Domain;

namespace EnergieBewustLeven.API.Data
{
    public class EnergieBewustLevenDbContext : DbContext
    {
        public EnergieBewustLevenDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
    }
}
